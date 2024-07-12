using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Models
{
    public class BaseModel<T> : BaseResponseModel
    {
        public T? Value { get; set; }
    }
}
