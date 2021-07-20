using API.Entities;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity;
using API.Cores;
using API.Database;
using API.Services.Interfaces;
using SqlKata;
using ColumnAttribute = SqlKata;
using API.Helpers;
using Dapper;

namespace API.Services
{
    public class GeneralService<TEntity> : IGeneralService<TEntity> where TEntity : BaseEntity, new()
    {
        [Dependency]
        public IUserService UserService { get; set; }
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        [Dependency]
        public DatabaseContext Context { get; set; }
        private string TableName
        {
            get
            {
                var type = typeof(TEntity);
                var table = type.GetCustomAttributes(true).Select(x => x as TableAttribute).FirstOrDefault(x => x != null);
                return table.Name;
            }
        }


        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await this.FindBy().GetAsync<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity, IDbTransaction transaction = null)
        {
            await Context.Factory.Query(this.TableName).InsertAsync(entity, transaction);
            var id = await Context.Factory.Connection.ExecuteScalarAsync<int>("SELECT last_insert_id() as Id", transaction: transaction);
            entity.Id = id;
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, IDbTransaction transaction = null)
        {
            await Context.Factory.Query(this.TableName).UpdateAsync(entity, transaction);
            return entity;
        }

        public Query FindBy(object predicate)
        {
            var t = predicate.GetType();
            var properties = t.GetProperties().Select(x =>
            {
                var col = x.GetCustomAttributes(typeof(SqlKata.ColumnAttribute), true).FirstOrDefault(x => x != null);
                return new
                {
                    key = col?.ToString() ?? x.Name,
                    value = x.GetValue(predicate)
                };
            });
            var query = this.FindBy();
            foreach (var item in properties)
            {
                query = query.Where(item.key, item.value);
            }
            return query;
        }

        public Query FindBy()
        {
            Dapper.SqlMapper.SetTypeMap(
            typeof(TEntity),
            new CustomPropertyTypeMap(
                typeof(TEntity),
                (type, columnName) =>
                    type.GetProperties().FirstOrDefault(prop =>
                        prop.GetCustomAttributes(false)
                            .OfType<SqlKata.ColumnAttribute>()
                            .Any(attr => attr.Name == columnName))));
            return this.Context.Factory.Query(this.TableName);
        }

        public async Task<TEntity> SingleBy(object predicate, IDbTransaction transaction = null)
        {
            var query = await this.FindBy(predicate).GetAsync<TEntity>(transaction);
            var result = query.FirstOrDefault();
            return result;
        }

        public async Task DeleteAsync(TEntity entity, IDbTransaction transaction = null)
        {
            await this.Context.Factory.Query(this.TableName).Where("id", entity.Id).DeleteAsync(transaction);
        }


    }
}
