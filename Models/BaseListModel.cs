using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Models
{
    public class BaseListModel<T> : BaseResponseModel
    {
        public List<T>? List { get; set; }
    }
}
