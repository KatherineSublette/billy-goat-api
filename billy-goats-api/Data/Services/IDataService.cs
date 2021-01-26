using System.Collections.Generic;
using System.Threading.Tasks;
using BillyGoats.Api.Data.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace BillyGoats.Api.Data.Services
{
    public interface IDataService<T> where T : class
    {
        IRepository<T> Repo { get; }

        void Save();
        
        Task<ICollection<T>> Get(params string[] includes);

        Task<T> GetById(long id, params string[] includes);

        Task<T> Add(T item);

        Task<T> Update(T item);

        Task<T> Patch(JsonPatchDocument<T> objectPatch, long id);

        Task Delete(long id);

        void Resolve(T entity, params string[] includePaths);
    }
}
