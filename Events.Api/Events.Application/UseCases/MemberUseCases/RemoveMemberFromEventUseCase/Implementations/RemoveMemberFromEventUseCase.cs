using Events.Application.Exceptions;
using Events.Application.Models.Member;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.MemberUseCases.RemoveMemberFromEventUseCase.Implementations;

internal class RemoveMemberFromEventUseCase : IRemoveMemberFromEventUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IValidator<DeleteAndAddMemberRequestDTO> validator;
    private readonly IUnitOfWork unitOfWork;

    public RemoveMemberFromEventUseCase(UserManager<Member> userManager,
        IValidator<DeleteAndAddMemberRequestDTO> validator,
        IUnitOfWork unitOfWork)
    {
        this.userManager = userManager;
        this.validator = validator;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
        unitOfWork.SetRegistrationRepository();
    }

    public async Task Execute(string eventId, string userName)
    {
        var result = validator.Validate(new DeleteAndAddMemberRequestDTO() { EventId = eventId, MemberName = userName });

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        var user = await userManager.FindByNameAsync(userName)
            ?? throw new ItemNotFoundException("User");

        var eventEntity = await unitOfWork.EventRepsository.FindById(eventId)
            ?? throw new ItemNotFoundException("Event");

        var registration = await unitOfWork.RegistrationRepository
            .FindByEventAndMemberIds(eventEntity.Id, user.Id)
            ?? throw new ItemNotFoundException("Registration");

        registration.Event.RemoveRegistation();

        unitOfWork.RegistrationRepository.Delete(registration);
        unitOfWork.EventRepsository.Update(registration.Event);
        await unitOfWork.Save();
    }
}
