using Dev.Business.Models;

namespace Dev.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsBySupplier(Guid supplierId);

        Task<IEnumerable<Product>> GetProductsSuppliers();

        Task<Product> GetProductSupplier(Guid id);
    }
}
