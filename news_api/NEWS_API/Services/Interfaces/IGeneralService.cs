using NEWS_API.Database;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NEWS_API.Services.Interfaces
{
    public interface IGeneralService<TEntity> where TEntity: BaseEntity, new()
    {
        DatabaseContext Context { get; set; }
        DbSet<TEntity> GetDbSet();
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> BulkInsert(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> BulkUpdate(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> BulkDelete(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleBy(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(TEntity entity);
        Task DeleteRange(IEnumerable<TEntity> entities);
        IGeneralService<TEntity> CheckUnique(string errorMessage);
        IGeneralService<TEntity> DisableLazyLoading();
    }
}
