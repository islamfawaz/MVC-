using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Common.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> _allowedExtensions = new() { ".png", ".jpg", ".jpeg" };
        private const int _allowedMaxSize = 2_097_152;

        public string? Upload(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName);

            // Check if the extension is allowed.
            if (!_allowedExtensions.Contains(extension))
                return null;

            // Check if the file size is within the allowed limit.
            if (file.Length > _allowedMaxSize)
                return null;

            // Ensure the folder exists or create it.
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generate a unique file name and save it.
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName; // Return the file name for storage in the database.
        }


        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}