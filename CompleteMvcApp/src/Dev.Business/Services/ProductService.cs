using Dev.Business.Interfaces;
using Dev.Business.Models;
using Dev.Business.Models.Validations;

namespace Dev.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository,
                              INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
        }

        public async Task Add(Product product)
        {
            if (!ValidationExecute(new ProductValidation(), product)) return;

            await _productRepository.Add(product);
        }

        public async Task Update(Product product)
        {
            if (!ValidationExecute(new ProductValidation(), product)) return;

            await _productRepository.Update(product);
        }

        public async Task Remove(Guid id)
        {
            await _productRepository.DeleteById(id);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}
