using Dev.Business.Models;

namespace Dev.Business.Interfaces
{
    public interface ISupplierService : IDisposable
    {
        Task Add(Supplier supplier);

        Task Update(Supplier supplier);

        Task Remove(Guid id);

        Task AddressUpdate(Address address);
    }
}
