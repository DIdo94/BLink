using BLink.Core.Constants;
using BLink.Core.Services;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using System.IO;
using System.Threading.Tasks;

namespace BLink.Services
{
    public class FileService : IFileService
    {
        public string CreateThumbnailFromImage(string path, string fileName)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string thumbnailPath = Path.Combine(
                    directoryPath,
                    $"{AppConstants.ThumbnailPrefix}-{fileName}");
            using (Image<Rgba32> thumbnail = Image.Load(path))
            {
                thumbnail.Mutate(x => x
                     .Resize(50, 50));
                thumbnail.Save(thumbnailPath);
            }

            return thumbnailPath;
        }

        public async Task SaveImage(string path, IFormFile image)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }
    }
}
