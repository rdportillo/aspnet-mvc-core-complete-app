﻿using AutoMapper;
using Dev.App.ViewModels;
using Dev.Business.Interfaces;
using Dev.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

            var imgPrefix = Guid.NewGuid() + "_";
            if(! await UploadFile(productViewModel.ImageUpload, imgPrefix))
            {
                return View(productViewModel);
            }

            productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;

            await _productRepository.Add(_mapper.Map<Product>(productViewModel));

            return RedirectToAction("Index");
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

            await _productRepository.Update(_mapper.Map<Product>(updatedProductViewModel));

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
