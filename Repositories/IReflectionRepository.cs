using System.Reflection;

namespace LibraryManagementSystem.Repositories
{
    public interface IReflectionRepository
    {
        public IQueryable<PropertyInfo> GetEntities();
    }
}
