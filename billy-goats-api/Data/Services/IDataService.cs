using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillyGoats.Api.Data.Repositories;
using BillyGoats.Api.Filter;
using Microsoft.AspNetCore.JsonPatch;

namespace BillyGoats.Api.Data.Services
{
    public interface IDataService<T> where T : class
    {
        IRepository<T> Repo { get; }

        void Save();
        
        Task<IQueryable<T>> Get(FilterRequest filter);

        Task<T> GetById(long id, params string[] includes);

        Task<T> Add(T item);

        Task<T> Update(T item);

        Task<T> Patch(JsonPatchDocument<T> objectPatch, long id);

        Task Delete(long id);

        void Resolve(T entity, params string[] includePaths);
    }
}
