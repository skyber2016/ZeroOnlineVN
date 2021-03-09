using SqlKata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IGeneralService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> AddAsync(TEntity entity, IDbTransaction transaction = null);

        Task<TEntity> UpdateAsync(TEntity entity, IDbTransaction transaction = null);
        Query FindBy();
        Query FindBy(object predicate);

        Task<TEntity> SingleBy(object predicate);

        Task Delete(TEntity entity, IDbTransaction transaction = null);
    }
}
