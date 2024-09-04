using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace UserManagement.Infrastructure.Persistence.Base
{
    public class RepositoryBase<TDbContext, TEntity> : IRepositoryBase<TEntity>
            where TDbContext : DbContext
            where TEntity : Entity
    {
        private readonly DbSet<TEntity> _dbEntity;

        private readonly ILogger logger;

        protected TDbContext Context { get; private set; }

        public IUnitOfWork UnitOfWork
        {
            get { return (IUnitOfWork)Context; }
        }

        public RepositoryBase(TDbContext dbContext, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(dbContext);
            ArgumentNullException.ThrowIfNull(logger);

            this.Context = dbContext;
            this.logger = logger;

            this._dbEntity = Context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IList<TEntity>> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (Context.Set<TEntity>() == null) throw new ArgumentNullException(nameof(entity));

            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Remove(TEntity entity)
        {
            if (Context.Set<TEntity>() == null) throw new ArgumentNullException(nameof(entity));

            Context.Set<TEntity>().Remove(entity);
        }

        public async Task RemoveRange(IList<TEntity> entities)
        {
            if (Context.Set<TEntity>() == null) throw new ArgumentNullException(nameof(entities));

            Context.Set<IList<TEntity>>().RemoveRange(entities);
        }
        public IQueryable<TEntity> Query()
        {
            return _dbEntity.AsQueryable();
        }

        public int Count()
        {
            return _dbEntity.Count();
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbEntity.CountAsync(cancellationToken);
        }

        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbEntity.Where(predicate).Any();
        }

        public async Task<bool> ExistAysnc(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbEntity.Where(predicate).AnyAsync(cancellationToken);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
        {
            return asNoTracking ? _dbEntity.AsNoTracking().SingleOrDefault(predicate) : _dbEntity.SingleOrDefault(predicate);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            TEntity entity = asNoTracking ? await _dbEntity.AsNoTracking().SingleOrDefaultAsync(predicate, cancellationToken) : await _dbEntity.SingleOrDefaultAsync(predicate, cancellationToken);

            return entity.CreateInstance<TEntity>();
        }

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
        {
            ICollection<TEntity> entities = asNoTracking ? _dbEntity.Where(predicate).AsNoTracking().ToList() : _dbEntity.Where(predicate).ToList();

            return entities.CreateInstance<ICollection<TEntity>>();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            ICollection<TEntity> entities = asNoTracking ? await _dbEntity.Where(predicate).AsNoTracking().ToListAsync(cancellationToken) : await _dbEntity.Where(predicate).ToListAsync(cancellationToken);

            return entities.CreateInstance<ICollection<TEntity>>();
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
        {
            IQueryable<TEntity> query = asNoTracking ? _dbEntity.Where(predicate).AsNoTracking() : _dbEntity.Where(predicate);

            return query.CreateInstance<IQueryable<TEntity>>();
        }

        public ICollection<TEntity> GetAll(bool asNoTracking = true)
        {
            ICollection<TEntity> entities = asNoTracking ? _dbEntity.AsNoTracking().ToList() : _dbEntity.ToList();

            return entities.CreateInstance<ICollection<TEntity>>();
        }

        public async Task<ICollection<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            ICollection<TEntity> entities = asNoTracking ? await _dbEntity.AsNoTracking().ToListAsync(cancellationToken) : await _dbEntity.ToListAsync(cancellationToken);

            return entities.CreateInstance<ICollection<TEntity>>();
        }

        public IList<TEntity> GetAllInclude(Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true)
        {
            IList<TEntity> entities = GetEntityWithNavigationProperty(asNoTracking, navigationProperties).ToList();

            return entities.CreateInstance<IList<TEntity>>();
        }

        public async Task<IList<TEntity>> GetAllIncludeAsync(Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            IList<TEntity> entities = await GetEntityWithNavigationProperty(asNoTracking, navigationProperties).ToListAsync(cancellationToken);

            return entities.CreateInstance<IList<TEntity>>();
        }

        public IList<TEntity> GetIncludeList(Func<TEntity, bool> predicate, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true)
        {
            IList<TEntity> entities = GetEntityWithNavigationProperty(asNoTracking, navigationProperties).AsEnumerable().Where(predicate).ToList<TEntity>();

            return entities.CreateInstance<IList<TEntity>>();
        }

        public TEntity GetSingleInclude(Func<TEntity, bool> predicate, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true)
        {
            TEntity entity = GetEntityWithNavigationProperty(asNoTracking, navigationProperties).FirstOrDefault(predicate);

            return entity.CreateInstance<TEntity>();
        }

        public async Task<TEntity> GetSingleIncludeAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            TEntity entity = await GetEntityWithNavigationProperty(asNoTracking, navigationProperties).SingleOrDefaultAsync(predicate, cancellationToken);

            return entity.CreateInstance<TEntity>();
        }

        public ICollection<TEntity> PaggedList(int? pageSize = 10, int? pageNumber = 1, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true)
        {
            IQueryable<TEntity> query = GetEntityWithNavigationProperty(asNoTracking, navigationProperties);

            if (pageNumber != null && pageSize != null)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public async Task<ICollection<TEntity>> PaggedListAsync(int? pageSize = 10, int? pageNumber = 1, Expression<Func<TEntity, object>>[]? navigationProperties = default, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetEntityWithNavigationProperty(asNoTracking, navigationProperties);

            if (pageNumber != null && pageSize != null)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public IList<TEntity> Filter(int? pageSize = 10, int? pageNumber = 1, Expression<Func<TEntity, bool>>? predicate = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default, string[]? includeProperties = default, bool asNoTracking = true)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (includeProperties != null)
            {
                foreach (string includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (pageNumber != null && pageSize != null) query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);


            return asNoTracking ? query.AsNoTracking().ToList() : query.ToList();
        }

        private IQueryable<TEntity> GetEntityWithNavigationProperty(bool asNoTracking, Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = this.Query();

            if (navigationProperties == null) navigationProperties = new Expression<Func<TEntity, object>>[] { };

            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
            {
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);
            }

            return asNoTracking ? dbQuery.AsNoTracking() : dbQuery;
        }
    }

    public static class CreateInstanceExtentions
    {
        public static Type CreateInstance<Type>(this Type type)
        {
            return type == null ? Activator.CreateInstance<Type>() : type;
        }
    }
}
