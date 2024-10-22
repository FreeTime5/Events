namespace Events.Application.Services.ImageService;
using Microsoft.AspNetCore.Http;

public interface IImageService
{
    Task UploadImage(string imagePath, IFormFile? formFile);

    string GetImagePath(IFormFile? formFile);

    string GetDefaultImagePath();

    Task DeleteImage(string imagePath);

    Task UpdateImage(string previousImagePath, string imagePath, IFormFile? formFile);

    string GetImageName(string imagePath);
}
