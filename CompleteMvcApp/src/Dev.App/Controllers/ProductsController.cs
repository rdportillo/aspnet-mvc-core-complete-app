using AutoMapper;
using Dev.App.ViewModels;
using Dev.Business.Interfaces;
using Dev.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dev.App.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
                                  ISupplierRepository supplierRepository,
                                  IMapper mapper)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = GetProduct(id);
            
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var productViewModel = await RetrieveSuppliers(new ProductViewModel());
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await RetrieveSuppliers(productViewModel);

            if (!ModelState.IsValid) return View(productViewModel);

            await _productRepository.Add(_mapper.Map<Product>(productViewModel));
            
            return View(productViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            
            if (productViewModel == null) return NotFound();
            
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();
            
            if (!ModelState.IsValid) return View(productViewModel);

            await _productRepository.Update(_mapper.Map<Product>(productViewModel));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            await _productRepository.DeleteById(id);
            
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
    }
}
