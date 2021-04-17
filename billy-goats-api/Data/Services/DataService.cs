using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BillyGoats.Api.Data.Repositories;
using BillyGoats.Api.Utils;
using BillyGoats.Api.Utils.Extensions;
using BillyGoats.Api.Filter;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BillyGoats.Api.Data.Services
{
    public class DataService<T> : IDataService<T> where T : class
    {
        public IRepository<T> Repo { get; }

        public DataService(IRepository<T> repo)
        {
            this.Repo = repo;
        }

        public virtual void Save()
        {
            this.Repo.Commit();
        }

        public virtual async Task<T> GetById(long id, params string[] includePaths)
        {
            return await this.Repo.GetByIdAsync(id, includePaths);
        }

        public virtual async Task<IQueryable<T>> Get(FilterRequest filter = null)
        {
            var query = await this.Repo.GetAllAsync();

            // use AsNoTracking() for performance
            query = query.AsNoTracking();

            if (filter == null)
            {
                return query;
            }
            else
            {
                return filter.ApplyFilter<T>(query);

            }
        }

        public virtual async Task<T> Patch(JsonPatchDocument<T> objectPatch, long id)
        {
            var objectToPatch = this.Repo.GetById(id);

            objectPatch.ApplyTo(objectToPatch);

            return await this.Repo.PatchAsync(objectToPatch);
        }

        public virtual async Task<T> Add(T item)
        {
            return await this.Repo.AddAsync(item, true);
        }

        public virtual async Task<T> Update(T item)
        {
            return await this.Repo.UpdateAsync(item, true);
        }

        public virtual async Task Delete(long id)
        {
            var data = this.Repo.GetById(id);
            if (data != null && CanDelete(id))
            {
                await this.Repo.DeleteAsync(data, true);
            }
        }

        public virtual bool CanDelete(long id)
        {
            return true;
        }

        public virtual void Resolve(T entity, params string[] includePaths)
        {

        }
    }
}
