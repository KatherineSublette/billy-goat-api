using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using BillyGoats.Api.Data.Helper;
using BillyGoats.Api.Models;
using BillyGoats.Api.Utils;
using BillyGoats.Api.Utils.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BillyGoats.Api.Data.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public DbContext Context { get; }

        protected DbSet<T> DbSet { get; set; }

        protected string[] NavigationProperties
        {
            get
            {
                return Context.Model.GetEntityTypes().SingleOrDefault(t => t.Name == typeof(T).FullName)
                .GetNavigations().Select(x => x.PropertyInfo.Name).ToArray();
            }
        }

        protected string[] AlwaysIncludeNavigationProperties
        {
            get
            {
                return Context.Model.GetEntityTypes().SingleOrDefault(t => t.Name == typeof(T).FullName)
                .GetNavigations().Where(x=> x.PropertyInfo.GetCustomAttribute<AlwaysIncludeAttribute>() != null)
                .Select(x => x.PropertyInfo.Name).ToArray();
            }
        }

        public EFRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            this.Context = dbContext;
            this.DbSet = this.Context.Set<T>();
        }

        public virtual IQueryable<T> GetAll(params string[] includePaths)
        {
            var res = this.DbSet as IQueryable<T>;

            var navProps = NavigationProperties;
            var defaultPaths = AlwaysIncludeNavigationProperties;
            if (includePaths.Contains("All"))
            {
                foreach (var nav in navProps)
                {
                    res = res.Include(nav);
                }
            }
            else 
            {
                foreach (var propName in AlwaysIncludeNavigationProperties)
                {
                    res = res.Include(propName);
                }
                foreach (var path in includePaths)
                {
                    var propName = path.CamelToPascalCase();
                    if (navProps.Contains(propName) && !defaultPaths.Contains(propName))
                    {
                        res = res.Include(propName);
                    }
                }
            }
            return res;
        }

        public virtual async Task<IQueryable<T>> GetAllAsync(params string[] includePaths)
        {
            return await Task.Run(() => { return this.GetAll(includePaths); });
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            // use AsNoTracking() for performance
            IQueryable<T> query = this.GetAll().AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                includeProperties.ForEach(i =>
                    query = query.Include(i));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
          
            return query;
        }

        public virtual async Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Task.Run(() => { return this.Get(filter, orderBy, includeProperties, page, pageSize); });
        }

        public virtual T GetById(int id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeys(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeys(null, id);
            }
        }

        public virtual Task<T> GetByIdAsync(int id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeysAsync(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeysAsync(null, id);
            }
        }

        public virtual T GetById(long id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeys(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeys(null, id);
            }
        }

        public virtual Task<T> GetByIdAsync(long id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeysAsync(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeysAsync(null, id);
            }
        }

        public virtual T GetById(Guid id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeys(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeys(null, id);
            }
        }

        public virtual Task<T> GetByIdAsync(Guid id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeysAsync(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeysAsync(null, id);
            }
        }

        public virtual T GetById(string id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeys(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeys(null, id);
            }
        }

        public virtual Task<T> GetByIdAsync(string id, params string[] includePaths)
        {
            if (includePaths.Length > 0)
            {
                return this.GetByKeysAsync(this.GetAll(includePaths), id);
            }
            else
            {
                return this.GetByKeysAsync(null, id);
            }
        }

        public virtual T GetByKeys(IQueryable<T> preFilter = null, params object[] keys)
        {
            EntityHelper.ConvertKeyValues<T>(keys);
            var item = this.DbSet.Find(keys);
            if (preFilter == null || preFilter.Contains(item))
            {
                return item;
            }

            return null;
        }

        public virtual T Add(T entity, bool immediatedCommit = false)
        {
            this.BeforeAdd(entity);
            var de = this.Context.Entry(entity);
            if (de.State != EntityState.Detached)
            {
                de.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }

            if (immediatedCommit)
            {
                this.Commit();
            }

            return entity;
        }

        public virtual T Update(T entity, bool immediatedCommit = false, string[] includeFields = null, string[] excludeFields = null)
        {
            if (entity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            this.BeforeUpdate(entity);

            var entry = this.Context.Entry<T>(entity);
            if (entry.State == EntityState.Detached)
            {
                var keyValues = entity.GetKeyValues();

                // You need to have access to key
                var attachedEntity = this.Context.Set<T>().Find(keyValues);
                if (attachedEntity != null)
                {
                    var attachedEntry = this.Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                    if (includeFields != null)
                    {
                        foreach (var field in includeFields)
                            attachedEntry.Property(field).IsModified = true;
                    }
                    else if (excludeFields != null)
                    {
                        foreach (var field in excludeFields)
                            attachedEntry.Property(field).IsModified = false;
                    }
                    entity = attachedEntity;
                }
                else
                {
                    this.DbSet.Attach(entity);
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }

            if (immediatedCommit)
            {
                this.Commit();
            }

            return entity;
        }

        public virtual void Delete(T entity, bool immediatedCommit = false)
        {
            if (entity == null)
            {
                return;
            }

            this.BeforeDelete(entity);
            var de = this.Context.Entry(entity);
            if (de.State != EntityState.Deleted)
            {
                de.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }

            if (immediatedCommit)
            {
                this.Commit();
            }
        }

        public virtual void Delete(long id, bool immediatedCommit = false)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return;
            }

            this.Delete(entity);

            if (immediatedCommit)
            {
                this.Commit();
            }
        }

        public virtual void Delete(int id, bool immediatedCommit = false)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return;
            }

            this.Delete(entity);

            if (immediatedCommit)
            {
                this.Commit();
            }
        }

        public virtual void Delete(Guid id, bool immediatedCommit = false)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return;
            }

            this.Delete(entity);
            if (immediatedCommit)
            {
                this.Commit();
            }
        }

        public virtual void Delete(string id, bool immediatedCommit = false)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return;
            }

            this.Delete(entity);
            if (immediatedCommit)
            {
                this.Commit();
            }
        }

        public virtual T Patch(T patchedObject)
        {
            this.Context.SaveChanges();

            return patchedObject;
        }

        public virtual async Task<T> PatchAsync(T entity)
        {
            return await Task.Run(() =>
            {
                return this.Patch(entity);
            });
        }

        public virtual void Attach(T entity)
        {
            var de = this.Context.Entry(entity);
            if (de.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
        }

        public virtual void Commit()
        {
            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                var exp = e;
                while (exp.InnerException != null)
                {
                    exp = exp.InnerException;
                }
                throw new ApplicationException(exp.Message);
            }
        }

        protected virtual async Task CommitAsync()
        {
            try
            {
                await this.Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var exp = e;
                while (exp.InnerException != null)
                {
                    exp = exp.InnerException;
                }
                throw new ApplicationException(exp.Message);
            }
        }

        protected virtual void BeforeAdd(T entity)
        {
        }

        protected virtual void BeforeUpdate(T entity)
        {
        }

        protected virtual void BeforeDelete(T entity)
        {
        }

        public virtual async Task<T> GetByKeysAsync(IQueryable<T> preFilter, params object[] keys)
        {
            EntityHelper.ConvertKeyValues<T>(keys);
            var item = await this.DbSet.FindAsync(keys);
            if (preFilter != null && !preFilter.Contains(item))
            {
                return null;
            }
            else
            {
                return item;
            }
        }

        public virtual async Task<T> AddAsync(T entity, bool immediatedCommit = false)
        {
            return await Task.Run(() =>
            {
                return this.Add(entity, immediatedCommit);
            });
        }

        public virtual async Task<T> UpdateAsync(T entity, bool immediatedCommit = false, string[] includeFields = null, string[] excludeFields = null)
        {
            return await Task.Run(() =>
            {
                return this.Update(entity, immediatedCommit, includeFields, excludeFields);
            });
        }

        public virtual async Task DeleteAsync(T entity, bool immediatedCommit = false)
        {
            await Task.Run(() =>
            {
                this.Delete(entity, immediatedCommit);
            });
        }

        public virtual async Task DeleteAsync(long id, bool immediatedCommit = false)
        {
            await Task.Run(() =>
            {
                this.Delete(id, immediatedCommit);
            });
        }

        public virtual async Task DeleteAsync(int id, bool immediatedCommit = false)
        {
            await Task.Run(() =>
            {
                this.Delete(id, immediatedCommit);
            });
        }

        public async Task DeleteAsync(Guid id, bool immediatedCommit = false)
        {
            await Task.Run(() =>
            {
                this.Delete(id, immediatedCommit);
            });
        }

        public async Task DeleteAsync(string id, bool immediatedCommit = false)
        {
            await Task.Run(() =>
            {
                this.Delete(id, immediatedCommit);
            });
        }
    }
}
