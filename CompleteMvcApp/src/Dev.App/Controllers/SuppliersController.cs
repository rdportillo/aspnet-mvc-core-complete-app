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
    [Route("suppliers")]
    public class SuppliersController : BaseController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   ISupplierService supplierService, 
                                   IMapper mapper,
                                   INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("list")]
        public async Task<IActionResult> Index()
        {
              return View(_mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll()));
        }

        [AllowAnonymous]
        [Route("details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null)
            {
                return NotFound();
            }

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Suppliers", "Add")]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Suppliers", "Add")]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Add(supplier);

            if (!ValidOperation()) return View(supplierViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Suppliers", "Edit")]
        [Route("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplierViewModel = await GetSupplierProductsAddress(id);

            if (supplierViewModel == null)
            {
                return NotFound();
            }
            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Suppliers", "Edit")]
        [Route("edit/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Update(supplier);

            if(!ValidOperation()) return View(await GetSupplierProductsAddress(id));

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Suppliers", "Delete")]
        [Route("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null) return NotFound();

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Suppliers", "Delete")]
        [Route("delete/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null) return NotFound();

            await _supplierService.Remove(id);

            if (!ValidOperation()) return View(supplierViewModel);
            
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Route("get-address/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if(supplier == null) return NotFound();

            return PartialView("_AddressDetails", supplier);
        }

        [ClaimsAuthorize("Suppliers", "Edit")]
        [Route("update-address/{id:guid}")]
        public async Task<IActionResult> AddressUpdate(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if(supplier == null) return NotFound();

            return PartialView("_AddressUpdate", new SupplierViewModel { Address = supplier.Address });
        }

        [ClaimsAuthorize("Suppliers", "Edit")]
        [Route("update-address/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AddressUpdate(SupplierViewModel supplierViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if(!ModelState.IsValid) return PartialView("_AddressUpdate", supplierViewModel);

            await _supplierService.AddressUpdate(_mapper.Map<Address>(supplierViewModel.Address));

            if (!ValidOperation()) return PartialView("_AddressUpdate", supplierViewModel);

            var url = Url.Action("GetAddress", "Suppliers", new { id = supplierViewModel.Address.SupplierId });
            return Json(new { success = true, url });
        }

        private async Task<SupplierViewModel> GetSupplierAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddress(id));
        }

        private async Task<SupplierViewModel> GetSupplierProductsAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddress(id));
        }
    }
}
