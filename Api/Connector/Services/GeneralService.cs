using Forum_API.Database;
using Forum_API.Entities;
using Forum_API.Security;
using Forum_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Services
{
    public class GeneralService<TEntity> : IGeneralService<TEntity> where TEntity : BaseEntity, new()
    {
        [Dependency]
        public IUserService UserService { get; set; }
        [Dependency]
        public DatabaseContext DatabaseContext { get; set; }
        private DbSet<TEntity> Context { get; set; }
        private UserPrincipal CurrentUser
        {
            get
            {
                return this.UserService.GetCurrentUser();
            }
        }
        private void SetContext()
        {
            if(this.Context == null)
                this.Context = this.DatabaseContext.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await this.FindBy(_=>true).ToListAsync();
        }
        
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            this.SetContext();
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.CreatedBy = this.CurrentUser?.Id;
            entity.UpdatedBy = this.CurrentUser?.Id;
            await this.Context.AddAsync(entity);
            await this.DatabaseContext.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            this.SetContext();
            var resp = entity;
            if (entity.Any())
            {
                var e = entity.Select(item =>
                {
                    item.CreatedDate = DateTime.Now;
                    item.UpdatedDate = DateTime.Now;
                    item.CreatedBy = this.CurrentUser?.Id;
                    item.UpdatedBy = this.CurrentUser?.Id;
                    return item;
                }).ToList();
                await Context.AddRangeAsync(e);
                await this.DatabaseContext.SaveChangesAsync();
                resp = e;
            }
            return resp;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            this.SetContext();
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = this.CurrentUser?.Id;
            Context.Update(entity);
            await this.DatabaseContext.SaveChangesAsync();
            return entity;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            this.SetContext();
            return this.Context.Where(predicate).Where(x => !x.IsDeleted);
        }

        public TEntity SingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            this.SetContext();
            return this.FindBy(predicate).FirstOrDefault();
        }

        public async Task Delete(TEntity entity)
        {
            this.SetContext();
            this.Context.Remove(entity);
            await this.DatabaseContext.SaveChangesAsync();
        }

    }
}
