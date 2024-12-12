using Events.Application.Exceptions;
using Events.Application.Services.ImageService;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.EventUseCases.DeleteEventUseCase.Implementations;

internal class DeleteEventUseCase : IDeleteEventUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IImageService imageService;
    private readonly IUnitOfWork unitOfWork;

    public DeleteEventUseCase(UserManager<Member> userManager,
        IImageService imageService,
        IUnitOfWork unitOfWork)
    {
        this.userManager = userManager;
        this.imageService = imageService;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
    }

    public async Task Execute(string eventId, string userName)
    {
        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        var role = (await userManager.GetRolesAsync(user)).First();

        var eventInstance = await unitOfWork.EventRepsository
            .FirstOrDefault(e => e.Id == eventId)
            ?? throw new ItemNotFoundException("Event");

        if (role != "Admin" && eventInstance.CreatorId != user.Id)
        {
            throw new UserHaveNoPermissionException();
        }

        string imagePath = eventInstance!.ImageUrl!;
        await imageService.DeleteImage(imagePath);

        unitOfWork.EventRepsository.Delete(eventInstance);
        await unitOfWork.Save();
    }
}
