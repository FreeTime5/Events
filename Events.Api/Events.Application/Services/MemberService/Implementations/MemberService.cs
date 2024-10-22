using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Member;
using Events.Infrastructure.Entities;
using Events.Infrastructure.UnitOfWorkPattern;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Services.MemberService.Implementations;

internal class MemberService : IMemberService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<MemberDb> userManager;
    private readonly IMapper mapper;
    private readonly IValidator<DeleteAndAddMemberRequestDTO> deleteAndAddValidator;

    public MemberService(IUnitOfWork unitOfWork,
        UserManager<MemberDb> userManager,
        IMapper mapper,
        IValidator<DeleteAndAddMemberRequestDTO> deleteAndAddValidator)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
        this.deleteAndAddValidator = deleteAndAddValidator;
    }

    public async Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(string eventId)
    {
        var eventEntity = await unitOfWork.EventRepository.GetById(eventId) ?? throw new ItemNotFoundException("Event");

        var membersOfEvent = unitOfWork.MemberRepository.GetAllFromEvent(eventEntity);

        var membersDTOs = MapMembers(membersOfEvent);

        return membersDTOs;
    }

    public async Task DeleteMemberFromEvent(string eventId, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");
        var result = deleteAndAddValidator.Validate(new DeleteAndAddMemberRequestDTO() { EventId = eventId, MemberId = user.Id });
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        var registration = await unitOfWork.RegistrationRepository.Find(user.Id, eventId) ?? throw new ItemNotFoundException("Registration");

        registration.Event.RemoveRegistation();

        await unitOfWork.RegistrationRepository.Remove(registration);
    }

    public async Task AddMemberToEvent(string eventId, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");
        var result = deleteAndAddValidator.Validate(new DeleteAndAddMemberRequestDTO() { EventId = eventId, MemberId = user.Id });

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        var eventEntity = await unitOfWork.EventRepository.GetById(eventId) ?? throw new ItemNotFoundException("Event");

        var registration = await unitOfWork.RegistrationRepository.Find(user.Id, eventEntity.Id);
        if (registration != null)
        {
            throw new InvalidOperationException("This user is already a member of the event");
        }

        var registrationResult = eventEntity.AddRegistration();
        if (!registrationResult)
        {
            throw new InvalidOperationException("Event has max number of members");
        }

        await unitOfWork.MemberRepository.AddToEvent(user, eventEntity);
    }

    public async Task UpdateMemberInformation(UpdateMemberDTO requestDTO, string userName)
    {

        var user = await unitOfWork.MemberRepository.GetByName(userName) ?? throw new ItemNotFoundException("User");

        user = UpdateUser(user, requestDTO);

        await unitOfWork.MemberRepository.Update(user);
    }

    private MemberDb UpdateUser(MemberDb user, UpdateMemberDTO requestDTO)
    {
        user.Birthday = requestDTO.Birthday == default ? user.Birthday : requestDTO.Birthday;
        user.FirstName = requestDTO.FirstName ?? user.FirstName;
        user.LastName = requestDTO.LastName ?? user.LastName;

        return user;
    }

    private IEnumerable<GetMemberDTO> MapMembers(IEnumerable<MemberDb> users)
    {
        return users.Select(MapMember);
    }

    private GetMemberDTO MapMember(MemberDb user)
    {
        var userDTO = mapper.Map<GetMemberDTO>(user);
        return userDTO;
    }
}
