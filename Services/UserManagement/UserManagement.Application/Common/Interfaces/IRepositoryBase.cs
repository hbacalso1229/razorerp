using UserManagement.Domain.SeedWork;
using System.Linq.Expressions;

namespace UserManagement.Application.Common.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        //TODO Command
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task<IList<TEntity>> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task Remove(TEntity entity);

        Task RemoveRange(IList<TEntity> entities);


        //TODO Query
        IQueryable<TEntity> Query();

        int Count();

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        bool Exist(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistAysnc(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        TEntity Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default);

        ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

        ICollection<TEntity> GetAll(bool asNoTracking = true);

        Task<ICollection<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default);

        IList<TEntity> GetAllInclude(Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true);

        Task<IList<TEntity>> GetAllIncludeAsync(Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true, CancellationToken cancellationToken = default);

        IList<TEntity> GetIncludeList(Func<TEntity, bool> predicate, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true);

        TEntity GetSingleInclude(Func<TEntity, bool> predicate, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true);

        Task<TEntity> GetSingleIncludeAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true, CancellationToken cancellationToken = default);

        ICollection<TEntity> PaggedList(int? pageSize = 10, int? pageNumber = 1, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true);

        Task<ICollection<TEntity>> PaggedListAsync(int? pageSize = 10, int? pageNumber = 1, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true, CancellationToken cancellationToken = default);

        IList<TEntity> Filter(int? pageSize = 10, int? pageNumber = 1, Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default, string[]? includeProperties = default, bool asNoTracking = true);
    }
}
