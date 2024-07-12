namespace LibraryManagementSystem.ViewModels
{
    public class FilterOptions
    {
        public int SortBy { get; set; } = 0;
        public string SearchQuery { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
