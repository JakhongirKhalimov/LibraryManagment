
using LibraryManagementSystem.Context;
using System.Reflection;

namespace LibraryManagementSystem.Repositories
{
    public class ReflectionRepository : IReflectionRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion
        #region Constructor
        public ReflectionRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Methods
        public IQueryable<PropertyInfo> GetEntities()
        {
            return _context.GetType().GetProperties().AsQueryable();
        }
        #endregion
    }
}
