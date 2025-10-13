using Ardalis.Specification;

namespace eShop.Catalog.API.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
