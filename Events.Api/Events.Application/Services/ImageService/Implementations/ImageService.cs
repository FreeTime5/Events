using Microsoft.AspNetCore.Http;

namespace Events.Application.Services.ImageService.Implementations;

internal class ImageService : IImageService
{
    private readonly string defaultImagePath;
    private readonly string imageFolder;

    public ImageService(string defaultImagePath, string imageFolder)
    {
        this.defaultImagePath = defaultImagePath;
        this.imageFolder = imageFolder;
    }

    public async Task DeleteImage(string imagePath)
    {
        if (imagePath == defaultImagePath)
            return;
        await Task.Run(() => File.Delete(imagePath));
    }

    public string GetDefaultImagePath()
    {
        return defaultImagePath;
    }

    public string GetImagePath(IFormFile? formFile)
    {
        if (formFile == null)
        {
            return defaultImagePath;
        }
        return Path.Combine(imageFolder, Guid.NewGuid().ToString() + "_" + formFile.FileName);
    }

    public async Task UpdateImage(string previousImagePath, string imagePath, IFormFile? formFile)
    {
        if (previousImagePath != defaultImagePath)
        {
            await DeleteImage(previousImagePath);
        }

        if (imagePath == defaultImagePath)
        {
            return;
        }

        await UploadImage(imagePath, formFile);
    }

    public async Task UploadImage(string imagePath, IFormFile? formFile)
    {
        if (imagePath == defaultImagePath)
        {
            return;
        }

        using var fileStream = new FileStream(imagePath, FileMode.Create);
        await formFile!.CopyToAsync(fileStream);
        fileStream.Close();
    }
}
