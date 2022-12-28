using AutoMapper;
using Dev.App.Attributes;
using Dev.App.ViewModels;
using Dev.Business.Interfaces;
using Dev.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dev.App.Controllers
{
    [Authorize]
    [Route("products")]
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
                                  ISupplierRepository supplierRepository,
                                  IMapper mapper,
                                  IProductService productService,
                                  INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _productService = productService;
        }

        [AllowAnonymous]
        [Route("list")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers()));
        }

        [AllowAnonymous]
        [Route("details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);
            
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Products", "Add")]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var productViewModel = await RetrieveSuppliers(new ProductViewModel());
            return View(productViewModel);
        }

        [ClaimsAuthorize("Products", "Add")]
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await RetrieveSuppliers(productViewModel);

            if (!ModelState.IsValid) return View(productViewModel);

            var imgPrefix = Guid.NewGuid() + "_";
            if(! await UploadFile(productViewModel.ImageUpload, imgPrefix))
            {
                return View(productViewModel);
            }

            productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;

            await _productService.Add(_mapper.Map<Product>(productViewModel));

            if(!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Products", "Edit")]
        [Route("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            
            if (productViewModel == null) return NotFound();
            
            return View(productViewModel);
        }

        [ClaimsAuthorize("Products", "Edit")]
        [Route("edit/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            var updatedProductViewModel = await GetProduct(id);
            productViewModel.Supplier = updatedProductViewModel.Supplier;
            productViewModel.Image = updatedProductViewModel.Image;

            if (!ModelState.IsValid) return View(productViewModel);

            if(productViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";

                if (!await UploadFile(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }

                updatedProductViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;
            }

            updatedProductViewModel.Name = productViewModel.Name;
            updatedProductViewModel.Description = productViewModel.Description;
            updatedProductViewModel.Price = productViewModel.Price;
            updatedProductViewModel.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(updatedProductViewModel));

            if(!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Products", "Delete")]
        [Route("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Products", "Delete")]
        [Route("delete/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            await _productService.Remove(id);

            if (!ValidOperation()) return View(productViewModel);

            TempData["Success"] = "Product deleted successfully!";
            
            return RedirectToAction("Index");
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplier(id));
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
            return product;
        }

        private async Task<ProductViewModel> RetrieveSuppliers(ProductViewModel product)
        {
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
            return product;
        }

        private async Task<bool> UploadFile(IFormFile file, string imagePrefix)
        {
            if(file.Length <= 0 ) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product_images", imagePrefix + file.FileName);

            if(System.IO.File.Exists(path))
            {
                ModelState.AddModelError(String.Empty, "There is already a file with the same name!");
                return false;
            }

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
