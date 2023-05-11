using ApplicationCore.Specifications;

namespace ApplicationCore.Interfaces
{
    public interface IRepository<T> 
    {
        public IDbManager DbManager { get;}

        public Task AddAsync(T t, CancellationToken cancellationToken = default);

        public Task DeleteAsync(T t, CancellationToken cancellationToken = default);

        public Task<List<T>> GetAllAsync(Specification<T>? specification = default, CancellationToken cancellationToken = default);

        public Task<bool> AnyAsync(Specification<T>? specification = default, CancellationToken cancellationToken = default);

        public Task<int> CountAsync(Specification<T>? specification = default, CancellationToken cancellationToken = default);
    }
}
