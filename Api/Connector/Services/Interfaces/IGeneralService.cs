using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Forum_API.Services.Interfaces
{
    public interface IGeneralService<TEntity>
    {
         Task<IEnumerable<TEntity>> GetAll();
         Task<TEntity> AddAsync(TEntity entity);
         Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entity);
         Task<TEntity> UpdateAsync(TEntity entity);
         IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
         TEntity SingleBy(Expression<Func<TEntity, bool>> predicate);
         Task Delete(TEntity entity);
    }
}
