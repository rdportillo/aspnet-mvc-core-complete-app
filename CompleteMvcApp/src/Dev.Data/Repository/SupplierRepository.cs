using Dev.Business.Interfaces;
using Dev.Business.Models;
using Dev.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Dev.Data.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext context) : base(context) { }

        public async Task<Supplier> GetSupplierAddress(Guid id)
        {
            return await Db.Suppliers.AsNoTracking()
                .Include(s => s.Address)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supplier> GetSupplierProductsAddress(Guid id)
        {
            return await Db.Suppliers.AsNoTracking()
                .Include(s => s.Address)
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
