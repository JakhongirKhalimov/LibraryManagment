namespace LibraryManagementSystem.Infastructure
{
    public partial class AppFileProvider : IAppFileProvider
    {
        #region Methods

        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public virtual string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*.*";

            return Directory.GetFileSystemEntries(directoryPath, searchPattern,
                new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = !topDirectoryOnly,
                });
        }
        #endregion
    }
}