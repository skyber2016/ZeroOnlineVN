using CORE_API.Cores;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NEWS_API.Attributes;
using NEWS_API.Cores.Exceptions;
using NEWS_MVC.Attributes;
using NEWS_MVC.Database;
using NEWS_MVC.Services.Interfaces;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity;

namespace NEWS_MVC.Services
{
    public class GeneralService<TEntity> : IGeneralService<TEntity> where TEntity : BaseEntity, new()
    {
        [Dependency]
        public IMemoryCache MemoryCache { get; set; }
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        [Dependency]
        public DatabaseContext Context { get; set; }
        
        private string UniqueMessage { get; set; }
        public DbSet<TEntity> GetDbSet()
        {
            return this.Context.Set<TEntity>();
        }
        public IGeneralService<TEntity> DisableLazyLoading()
        {
            this.Context.ChangeTracker.LazyLoadingEnabled = false;
            return this;
        }
        private void DetachLocal(TEntity t)
        {
            var context = this.Context;
            var local = context.Set<TEntity>()
                .Local
                .FirstOrDefault(entry => entry.GetHashCode() == t.GetHashCode());
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            else
            {
                context.Entry(t).State = EntityState.Modified;
            }
        }
        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;

        }
        [ResetCache]
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            CheckUnique(entity);
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            ResetCache();
            return entity;
        }
        private object ConvertToSingleObjeect(TEntity entity)
        {
            var obj = new ExpandoObject();
            var dic = obj as IDictionary<string, object>;
            var type = entity.GetType();
            var properties = type.GetProperties().Where(x => x.PropertyType.FullName.StartsWith("System") && !x.PropertyType.IsGenericType);
            foreach (var item in properties)
            {
                var ignore = item.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any();
                if (ignore)
                {
                    continue;
                }
                dic[item.Name] = item.GetValue(entity);
            }
            return obj;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
            ResetCache();
            return entity;
        }
        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.Set<TEntity>().Where(predicate);
        }

        public TEntity SingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.Set<TEntity>().Where(predicate).AsEnumerable().FirstOrDefault();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DetachLocal(entity);
            this.Context.Set<TEntity>().Remove(entity);
            await this.Context.SaveChangesAsync();
        }
        public async Task DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities.Any())
            {
                this.Context.Set<TEntity>().RemoveRange(entities);
                await this.Context.SaveChangesAsync();
            }
        }
        

        private void ResetCache()
        {
        }

        private void CheckUnique(TEntity entity)
        {
            if(this.UniqueMessage == null)
            {
                return;
            }
            var type = entity.GetType();
            var properties = type.GetProperties();
            string columnName = string.Empty;
            string tableName = string.Empty;
            var dict = new Dictionary<string, object>();
            var table = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes(typeof(UniqueAttribute), true).FirstOrDefault() as UniqueAttribute;
                
                var column = property.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
                if(column == null)
                {
                    continue;
                }
                dict[column.Name] = property.GetValue(entity);
                if (attr != null)
                {
                    columnName = column.Name;
                    tableName = table.Name;
                }
            }
            if(columnName == string.Empty || tableName == string.Empty)
            {
                return;
            }
            var dbSet = this.GetDbSet();
            var parameter = new OracleParameter[]
            {
                new OracleParameter("columnName", dict[columnName])
            };
            var dataExist = dbSet.FromSqlRaw($"SELECT {columnName} FROM {tableName} WHERE UPPER({columnName}) = UPPER(:columnName)", parameter).Any();
            if (dataExist)
            {
                throw new BadRequestException(this.UniqueMessage);
            }
        }

        public IGeneralService<TEntity> CheckUnique(string errorMessage)
        {
            this.UniqueMessage = errorMessage;
            return this;
        }

        public async Task<IEnumerable<TEntity>> BulkInsert(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;
            this.Context.Set<TEntity>().AddRange(entities);
            await this.Context.SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<TEntity>> BulkDelete(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;
            Context.Set<TEntity>().RemoveRange(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<TEntity>> BulkUpdate(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().UpdateRange(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

    }
}
