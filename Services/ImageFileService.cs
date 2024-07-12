using Microsoft.AspNetCore.Hosting;

namespace LibraryManagementSystem.Services
{
    public class ImageFileService : IImageFileService
    {
        #region Fields
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        #region Constructor
        public ImageFileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion
        #region Methods
        public string SaveImageFile(IFormFile imageFile)
        {
            string filePath = "images/book-covers/" + Guid.NewGuid().ToString() + "_" + imageFile.FileName;

            string serverFilePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

            using (var fileStream = new FileStream(serverFilePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return "/" + filePath;
        }
        #endregion
    }
}
