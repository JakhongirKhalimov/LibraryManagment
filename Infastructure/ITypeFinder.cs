using System.Reflection;

namespace LibraryManagementSystem.Infastructure
{
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);
        IList<Assembly> GetAssemblies();
    }
}
