using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _table;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _table.AsNoTracking();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _table.Where(expression).AsNoTracking();
        }

        public void Add(T entity)
        {
            try
            {
                _table.Add(entity);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public void Update(T entity)
        {
            _table.Update(entity);
        }

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            int isSuccessful = await _context.SaveChangesAsync();
            return isSuccessful > 0;
        }

        public bool SaveChanges()
        {
            int isSuccessful = _context.SaveChanges();
            return isSuccessful > 0;
        }
    }
}
