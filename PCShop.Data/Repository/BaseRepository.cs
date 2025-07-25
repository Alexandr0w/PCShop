using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using PCShop.Data.Repository.Interfaces;
using static PCShop.Data.Common.ExceptionMessages;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey>
        : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly PCShopDbContext _dbContext;
        protected readonly DbSet<TEntity> _DbSet;

        protected BaseRepository(PCShopDbContext _dbContext)
        {
            this._dbContext = _dbContext;
            this._DbSet = this._dbContext.Set<TEntity>();
        }

        public TEntity? GetById(TKey id)
        {
            return this._DbSet
                .Find(id);
        }

        public ValueTask<TEntity?> GetByIdAsync(TKey id)
        {
            return this._DbSet
                .FindAsync(id);
        }

        public TEntity? SingleOrDefault(Func<TEntity, bool> predicate)
        {
            return this._DbSet
                .SingleOrDefault(predicate);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this._DbSet
                .SingleOrDefaultAsync(predicate);
        }

        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return this._DbSet
                .FirstOrDefault(predicate);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this._DbSet
                .FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this._DbSet
                .ToArray();
        }

        public int Count()
        {
            return this._DbSet
                .Count();
        }

        public Task<int> CountAsync()
        {
            return this._DbSet
                .CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            TEntity[] entities = await this._DbSet
                .ToArrayAsync();

            return entities;
        }

        public IQueryable<TEntity> AllAsNoTracking()
        {
            return this._DbSet
                .AsNoTracking();
        }

        public IQueryable<TEntity> GetAllAttached()
        {
            return this._DbSet
                .AsQueryable();
        }

        public void Add(TEntity item)
        {
            this._DbSet.Add(item);
            this._dbContext.SaveChanges();
        }

        public async Task AddAsync(TEntity item)
        {
            await this._DbSet.AddAsync(item);
            await this._dbContext.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            this._DbSet.AddRange(items);
            this._dbContext.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await this._DbSet.AddRangeAsync(items);
            await this._dbContext.SaveChangesAsync();
        }

        public bool Delete(TEntity entity)
        {
            this.PerformSoftDeleteOfEntity(entity);

            return this.Update(entity);
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            this.PerformSoftDeleteOfEntity(entity);

            return this.UpdateAsync(entity);
        }

        public bool HardDelete(TEntity entity)
        {
            this._DbSet.Remove(entity);
            int updateCnt = this._dbContext.SaveChanges();

            return updateCnt > 0;
        }

        public async Task<bool> HardDeleteAsync(TEntity entity)
        {
            this._DbSet.Remove(entity);
            int updateCnt = await this._dbContext.SaveChangesAsync();

            return updateCnt > 0;
        }

        public bool Update(TEntity item)
        {
            try
            {
                this._DbSet.Attach(item);
                this._DbSet.Entry(item).State = EntityState.Modified;
                this._dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEntity item)
        {
            try
            {
                this._DbSet.Attach(item);
                this._DbSet.Entry(item).State = EntityState.Modified;
                await this._dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveChanges()
        {
            this._dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }

        private void PerformSoftDeleteOfEntity(TEntity entity)
        {
            PropertyInfo? isDeletedProperty =
                this.GetIsDeletedProperty(entity);
            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException(SoftDeleteOnNonSoftDeletableEntity);
            }

            isDeletedProperty.SetValue(entity, true);
        }

        private PropertyInfo? GetIsDeletedProperty(TEntity entity)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(pi => pi.PropertyType == typeof(bool) && pi.Name == IsDeletedPropertyName);
        }
    }
}