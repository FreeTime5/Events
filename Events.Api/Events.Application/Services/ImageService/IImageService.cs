using Microsoft.AspNetCore.Http;

namespace Events.Application.Services.ImageService;

public interface IImageService
{
    string GetImagePath(IFormFile? formFile);

    Task UploadImage(string imagePath, IFormFile? formFile);

    string GetDefaultImagePath();

    Task DeleteImage(string imagePath);

    Task UpdateImage(string previousImagePath, string imagePath, IFormFile? formFile);

    string GetImageName(string imagePath);
}
