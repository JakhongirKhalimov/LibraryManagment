using Microsoft.Extensions.FileProviders;

namespace LibraryManagementSystem.Infastructure
{
    public interface IAppFileProvider
    {
        bool DirectoryExists(string path);
        string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true);
    }
}