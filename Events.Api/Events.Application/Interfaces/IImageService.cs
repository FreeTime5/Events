using Events.Application.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface IImageService
{
    Task<Result> UploadImage(string imagePath, IFormFile? formFile);

    string GetImagePath(IFormFile? formFile);

    string GetDefaultImagePath();

    Task DeleteImage(string imagePath);

    Task<Result> UpdateImage(string previousImagePath, string imagePath, IFormFile? formFile);
}
