using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Application.Services.ImageService;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.EventUseCases.CreateEventUseCase.implementations;

internal class CreateEventUseCase : ICreateEventUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IValidator<CreateEventRequestDTO> createEventValidator;
    private readonly IImageService imageService;
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;

    public CreateEventUseCase(UserManager<Member> userManager,
        IValidator<CreateEventRequestDTO> createEventValidator,
        IImageService imageService,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        this.userManager = userManager;
        this.createEventValidator = createEventValidator;
        this.imageService = imageService;
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
        unitOfWork.SetCategoryRepository();
    }

    public async Task Execute(CreateEventRequestDTO eventRequestDTO, string userName)
    {
        var validationResult = await createEventValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        var sameEvent = await unitOfWork.EventRepsository
        .FirstOrDefault(e => e.Title == eventRequestDTO.Title);

        if (sameEvent != null)
        {
            throw new ItemAlreadyAddedException("Event");
        }

        var imagePath = imageService.GetImagePath(eventRequestDTO.Image);

        if (eventRequestDTO.CategoryId != null)
        {
            var category = await unitOfWork.CategoryRepository
                .FirstOrDefault(c => c.Name == eventRequestDTO.CategoryId)
                ?? throw new ItemNotFoundException("Category");
        }

        var eventInstance = mapper.Map<Event>(eventRequestDTO);

        eventInstance.Creator = user;
        eventInstance.ImageUrl = imagePath;
        eventInstance.ImageName = imageService.GetImageName(imagePath);

        unitOfWork.EventRepsository.Add(eventInstance);
        await unitOfWork.Save();
        await UploadImage(imagePath, eventRequestDTO.Image);
    }

    private async Task UploadImage(string imagePath, IFormFile? file)
    {
        if (!string.IsNullOrEmpty(imagePath))
        {
            await imageService.UploadImage(imagePath, file);
        }
    }
}
