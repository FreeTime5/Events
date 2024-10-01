using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Servicies.ImageService;

public class ImageService : IImageService
{
    private readonly string _defaultImagePath;
    private readonly string _imageFolder;

    public ImageService(string defaultImagePath, string imageFolder)
    {
        _defaultImagePath = defaultImagePath;
        _imageFolder = imageFolder;
    }

    public async Task DeleteImage(string imagePath)
    {
        if (imagePath == _defaultImagePath)
            return;
        await Task.Run(() => File.Delete(imagePath));
    }

    public string GetDefaultImagePath()
    {
        return _defaultImagePath;
    }

    public string GetImagePath(IFormFile? formFile)
    {
        if (formFile == null)
            return _defaultImagePath;
        return Path.Combine(_imageFolder, Guid.NewGuid().ToString() + "_" + formFile.FileName);
    }

    public async Task<Result> UpdateImage(string previousImagePath, string imagePath, IFormFile? formFile)
    {
        if (previousImagePath != _defaultImagePath)
            await DeleteImage(previousImagePath);

        if (imagePath == _defaultImagePath)
            return Result.Success();

        return await UploadImage(imagePath, formFile);
    }

    public async Task<Result> UploadImage(string imagePath, IFormFile? formFile)
    {
        if (imagePath == _defaultImagePath)
            return Result.Success();
        using var fileStream = new FileStream(imagePath, FileMode.Create);
        try
        {
            await formFile!.CopyToAsync(fileStream);
        }
        catch(Exception ex)
        {
            return Result.Failure([new Error(ex.Message, "", ex.Source ?? "")]);
        }
        finally
        {
            fileStream.Close();
        }
        return Result.Success();
    }
}
