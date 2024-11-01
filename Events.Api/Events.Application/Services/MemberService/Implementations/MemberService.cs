using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Member;
using Events.Domain.Entities;
using Events.Infrastructure.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Services.MemberService.Implementations;

internal class MemberService : Service, IMemberService
{
    private readonly IMapper mapper;
    private readonly IValidator<DeleteAndAddMemberRequestDTO> deleteAndAddValidator;
    private readonly UserManager<Member> userManager;

    public MemberService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<DeleteAndAddMemberRequestDTO> deleteAndAddValidator,
        UserManager<Member> userManager)
        :base(unitOfWork)
    {
        this.mapper = mapper;
        this.deleteAndAddValidator = deleteAndAddValidator;
        this.userManager = userManager;
    }

    public async Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(string eventId)
    {
        var eventEntity = await unitOfWork.GetRepository<Event>()
            .FirstOrDefaultAsNoTracking(e => e.Id == eventId) 
            ?? throw new ItemNotFoundException("Event");

        var membersOfEvent = unitOfWork.GetRepository<Registration>()
            .FindByAsNoTracking(r => r.EventId == eventId)
            .Select(r => r.Member);

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

        var registration = await unitOfWork.GetRepository<Registration>()
            .FirstOrDefault(r => r.MemberId == user.Id && r.EventId == eventId) 
            ?? throw new ItemNotFoundException("Registration");

        registration.Event.RemoveRegistation();

        unitOfWork.GetRepository<Registration>().Delete(registration);
        unitOfWork.GetRepository<Event>().Update(registration.Event);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task AddMemberToEvent(string eventId, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) 
            ?? throw new ItemNotFoundException("User");

        var result = deleteAndAddValidator.Validate(new DeleteAndAddMemberRequestDTO() { EventId = eventId, MemberId = user.Id });

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        var eventEntity = await unitOfWork.GetRepository<Event>()
            .FirstOrDefault(e => e.Id == eventId) 
            ?? throw new ItemNotFoundException("Event");

        var existedRegistration = await unitOfWork.GetRepository<Registration>()
                .FirstOrDefault(r => r.MemberId == user.Id && r.EventId == eventId);

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

        unitOfWork.GetRepository<Registration>().Add(registration);
        unitOfWork.GetRepository<Event>().Update(registration.Event);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateMemberInformation(UpdateMemberDTO requestDTO, string userName)
    {
        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        user = UpdateUser(user, requestDTO);

        unitOfWork.GetRepository<Member>().Update(user);
        await unitOfWork.SaveChangesAsync();
    }

    private Member UpdateUser(Member user, UpdateMemberDTO requestDTO)
    {
        user.Birthday = requestDTO.Birthday == default ? user.Birthday : requestDTO.Birthday;
        user.FirstName = requestDTO.FirstName ?? user.FirstName;
        user.LastName = requestDTO.LastName ?? user.LastName;

        return user;
    }

    private IEnumerable<GetMemberDTO> MapMembers(IEnumerable<Member> users)
    {
        return users.Select(MapMember);
    }

    private GetMemberDTO MapMember(Member user)
    {
        var userDTO = mapper.Map<GetMemberDTO>(user);
        return userDTO;
    }
}
