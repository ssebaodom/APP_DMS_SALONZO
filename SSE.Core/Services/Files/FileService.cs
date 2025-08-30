using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SSE.Core.Common.Constants;
using SSE.Core.Services.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SSE.Core.Services.Files
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment hostingEnv;
        private readonly IConfiguration configuration;

        public FileService(IWebHostEnvironment hostingEnv, IConfiguration configuration)
        {
            this.hostingEnv = hostingEnv;
            this.configuration = configuration;
        }

        public bool CheckFileExits(string filePath, ref Stream stream)
        {
            string fullPathFile = Path.Combine(this.hostingEnv.WebRootPath, filePath);
            if (System.IO.File.Exists(fullPathFile))
            {
                stream = this.hostingEnv
                    .WebRootFileProvider
                    .GetFileInfo(filePath)
                    .CreateReadStream();
                return true;
            }
            return false;
        }

        public string createPathFile(string path, IFormFile file)
        {

            string keyFileEncode = this.configuration[CONFIGURATION_KEYS.SECRET_KEY].ToString();
            string pathFile = CryptHelper.GetHashMD5(keyFileEncode + Guid.NewGuid().ToString() + file.FileName) + Path.GetExtension(file.FileName);
            var itemCheck = this.hostingEnv.WebRootPath;

            // string fullPath = Path.Combine("fsdUpload", pathFile);
            //var folderYear = DateTime.Now.Year.ToString();
            path = Path.Combine(path, DateTime.Now.Year.ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, DateTime.Now.ToString("MM"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, DateTime.Now.ToString("dd"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, pathFile);
            //return Path.Combine(fullPath);
        }

        public void SaveFile(IFormFile file, string filePath)
        {

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        public Task SaveFileAsync(IFormFile file, string filePath)
        {
            filePath = Path.Combine(this.hostingEnv.WebRootPath, filePath);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                return file.CopyToAsync(fileStream);
            }
        }
    }
}