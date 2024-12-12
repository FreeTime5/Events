using Events.Application.Exceptions;
using Events.Application.Models.Member;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.MemberUseCases.AddMemberToEventUseCase.Implementations;
internal class AddMemberToEventUseCase : IAddMemberToEventUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IValidator<DeleteAndAddMemberRequestDTO> validator;
    private readonly IUnitOfWork unitOfWork;

    public AddMemberToEventUseCase(UserManager<Member> userManager,
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

        var eventEntity = await unitOfWork.EventRepsository
            .FindById(eventId)
            ?? throw new ItemNotFoundException("Event");

        var existedRegistration = await unitOfWork.RegistrationRepository
                .FindByEventAndMemberIds(eventEntity.Id, user.Id);

        if (existedRegistration != null)
        {
            throw new InvalidOperationException("This user is already a member of the event");
        }

        var registrationResult = eventEntity.AddRegistration();

        if (!registrationResult)
        {
            throw new InvalidOperationException("Event has max number of members");
        }

        var registration = new Registration()
        {
            Member = user,
            Event = eventEntity,
            RegistrationDate = DateTime.UtcNow
        };

        unitOfWork.RegistrationRepository.Add(registration);
        unitOfWork.EventRepsository.Update(registration.Event);
        await unitOfWork.Save();
    }
}
