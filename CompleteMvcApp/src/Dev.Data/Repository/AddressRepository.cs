using Dev.Business.Interfaces;
using Dev.Business.Models;
using Dev.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Dev.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }

        public async Task<Address> GetAddressBySupplier(Guid id)
        {
            return await Db.Addresses.AsNoTracking()
                .FirstOrDefaultAsync(a => a.SupplierId == id);
        }
    }
}
