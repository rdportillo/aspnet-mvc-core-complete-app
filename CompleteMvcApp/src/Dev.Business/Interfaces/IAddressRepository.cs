using Dev.Business.Models;

namespace Dev.Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressBySupplier(Guid id);
    }
}
