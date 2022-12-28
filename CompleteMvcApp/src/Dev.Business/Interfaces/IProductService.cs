using Dev.Business.Models;

namespace Dev.Business.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task Add(Product product);

        Task Update(Product product);

        Task Remove(Guid id);
    }
}
