using Dev.Business.Models;

namespace Dev.Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier> GetSupplierAddress(Guid id);

        Task<Supplier> GetSupplierProductsAddress(Guid id);
    }
}
