using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace SSE.Core.Services.Files
{
    public interface IFileService
    {
        string createPathFile(string path, IFormFile file);

        void SaveFile(IFormFile file, string filePath);

        Task SaveFileAsync(IFormFile file, string filePath);

        bool CheckFileExits(string filePath, ref Stream stream);
    }
}