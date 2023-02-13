using NanoSingular.Application.Common.Marker;
using Microsoft.AspNetCore.Http;


namespace NanoSingular.Application.Common.Images
{
    public interface IImageService : ITransientService
    {
        Task<string> AddImage(IFormFile file, int height, int width);
        Task<string> DeleteImage(string url);
    }
}
