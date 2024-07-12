namespace LibraryManagementSystem.Models
{
    public class BaseResponseModel
    {
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; } = "";
    }
}
