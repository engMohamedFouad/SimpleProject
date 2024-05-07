using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool DeletePhysicalFile(string path)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + path);
            if (File.Exists(directoryPath))
            {
                File.Delete(directoryPath);
                return true;
            }
            return false;
        }

        public async Task<string> Upload(IFormFile file, string location)
        {

            try
            {
                var path = _webHostEnvironment.WebRootPath + location;    //wwwroot+images
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString().Replace("-", string.Empty) + extension;
                //Save
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = File.Create(path + fileName))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                    return $"{location}/{fileName}";
                }
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }
        }
    }
}
