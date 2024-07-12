using LibraryManagementSystem.Entities;
using LibraryManagementSystem.ViewModels;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T,bool>> expression);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        bool SaveChanges();
        Task<bool> SaveChangesAsync();
    }
}
