using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BillyGoats.Api.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbContext Context { get; }

        IQueryable<T> GetAll(params string[] includePaths);

        Task<IQueryable<T>> GetAllAsync(params string[] includePaths);

        IQueryable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<Expression<Func<T, object>>> includeProperties = null,
        int? page = null,
        int? pageSize = null);

        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<Expression<Func<T, object>>> includeProperties = null,
        int? page = null,
        int? pageSize = null);

        T GetById(int id, params string[] includePaths);

        Task<T> GetByIdAsync(int id, params string[] includePaths);

        T GetById(long id, params string[] includePaths);

        Task<T> GetByIdAsync(long id, params string[] includePaths);

        T GetById(Guid id, params string[] includePaths);

        Task<T> GetByIdAsync(Guid id, params string[] includePaths);

        T GetById(string id, params string[] includePaths);

        Task<T> GetByIdAsync(string id, params string[] includePaths);

        T GetByKeys(IQueryable<T> perFilter = null, params object[] keys);

        Task<T> GetByKeysAsync(IQueryable<T> preFilter = null, params object[] keys);

        T Add(T entity, bool immediatedCommit = false);

        Task<T> AddAsync(T entity, bool immediatedCommit = false);

        T Update(T entity, bool immediatedCommit = false, string[] includeFields  = null, string[] excludeFields = null);

        Task<T> UpdateAsync(T entity, bool immediatedCommit = false, string[] includeFields = null, string[] excludeFields = null);

        void Delete(T entity, bool immediatedCommit = false);

        Task DeleteAsync(T entity, bool immediatedCommit = false);

        void Delete(long id, bool immediatedCommit = false);

        Task DeleteAsync(long id, bool immediatedCommit = false);

        void Delete(int id, bool immediatedCommit = false);

        Task DeleteAsync(int id, bool immediatedCommit = false);

        void Delete(Guid id, bool immediatedCommit = false);

        Task DeleteAsync(Guid id, bool immediatedCommit = false);

        void Delete(string id, bool immediatedCommit = false);

        Task DeleteAsync(string id, bool immediatedCommit = false);

        T Patch(T patchedObject);

        Task<T> PatchAsync(T patchedObject);

        void Attach(T entity);

        void Commit();
    }
}
