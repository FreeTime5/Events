using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Application.Services.EmailService;
using Events.Application.Services.ImageService;
using Events.Application.UseCases.EventUseCases.GetAllUsersRegistredOnEventUseCase;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.EventUseCases.UpdateEventUseCase.Implementations;

internal class UpdateEventUseCase : IUpdateEventUseCase
{
    private readonly IValidator<UpdateEventRequestDTO> updateValidator;
    private readonly UserManager<Member> userManager;
    private readonly IMapper mapper;
    private readonly IGetAllUsersRegistredOnEventUseCase getAllUsersRegistredOnEvent;
    private readonly IImageService imageService;
    private readonly IEmailService emailService;
    private readonly IUnitOfWork unitOfWork;

    public UpdateEventUseCase(IValidator<UpdateEventRequestDTO> updateValidator,
        UserManager<Member> userManager,
        IMapper mapper,
        IGetAllUsersRegistredOnEventUseCase getAllUsersRegistredOnEvent,
        IImageService imageService,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        this.updateValidator = updateValidator;
        this.userManager = userManager;
        this.mapper = mapper;
        this.getAllUsersRegistredOnEvent = getAllUsersRegistredOnEvent;
        this.imageService = imageService;
        this.emailService = emailService;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
        unitOfWork.SetCategoryRepository();
    }

    public async Task Execute(UpdateEventRequestDTO eventRequestDTO, string userName)
    {
        var validationResult = await updateValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        var eventEntity = await unitOfWork.EventRepsository
            .FirstOrDefault(e => e.Id == eventRequestDTO.Id)
            ?? throw new ItemNotFoundException("Event");

        if (!await userManager.IsInRoleAsync(user, "Admin") && user.Id != eventEntity.CreatorId)
        {
            throw new UserHaveNoPermissionException();
        }

        string previousImagePath = eventEntity.ImageUrl;

        if (eventRequestDTO.CategoryId != null)
        {
            var category = await unitOfWork.CategoryRepository
                .FirstOrDefaultAsNoTracking(c => c.Name == eventRequestDTO.CategoryId) 
                ?? throw new ItemNotFoundException("Category");
        }

        mapper.Map(eventRequestDTO, eventEntity);

        unitOfWork.EventRepsository.Update(eventEntity);
        await unitOfWork.Save();

        await imageService.UpdateImage(previousImagePath, eventEntity.ImageUrl, eventRequestDTO.Image);

        var users = await getAllUsersRegistredOnEvent.Execute(eventEntity.Id);

        emailService.SendEmail(users, "Events", $"The event: {eventEntity.Title} has been updated");
    }
}
