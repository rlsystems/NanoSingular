using Microsoft.AspNetCore.Http;
using CloudinaryDotNet; // cloudinary sdk
using CloudinaryDotNet.Actions; // cloudinary sdk
using Microsoft.Extensions.Options;
using NanoSingular.Application.Common.Images;

// Cloudinary is a 3rd party platform for digital asset management.
// Images (and files) are stored on their platform and retrieved via public CDN
// This is an implementation of their service using the CloudinaryDotNet SDK
// Create a free account and change the api keys in appsettings.json

namespace NanoSingular.Infrastructure.Images
{
    public class CloudinaryService : IImageService
    {

        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> AddImage(IFormFile file, int height, int width)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(height).Width(width).Crop("fill").Gravity("auto")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }

        public async Task<string> DeleteImage(string url)
        {
            var urlSegment = new Uri(url).Segments.Last();
            var publicId = Path.GetFileNameWithoutExtension(urlSegment); 

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok" ? result.Result : null;
        }
    }
}
