using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BLink.Core.Services
{
    public interface IFileService
    {
        Task SaveImage(string path, IFormFile image);

        string CreateThumbnailFromImage(string path, string fileName);
    }
}
