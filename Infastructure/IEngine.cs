namespace LibraryManagementSystem.Infastructure
{
    public interface IEngine
    {
        void ConfigureRequestPipeline(IApplicationBuilder application);
        IEnumerable<T> ResolveAll<T>();
    }
}
