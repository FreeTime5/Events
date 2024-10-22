using AutoMapper;
using Events.Application.Models.Member;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Events.Infrastructure.UnitOfWorkPattern;
using FluentValidation;

namespace Events.Application.Services.MemberService.Implementations;

internal class MemberService : IMemberService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IValidator<DeleteAndAddMemberRequestDTO> deleteAndAddValidator;

    public MemberService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<DeleteAndAddMemberRequestDTO> deleteAndAddValidator)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.deleteAndAddValidator = deleteAndAddValidator;
    }

    public async Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(string eventId)
    {
        var membersOfEvent = await unitOfWork.MemberRepository.GetAllFromEvent(eventId);

        var membersDTOs = MapMembers(membersOfEvent);

        return membersDTOs;
    }

    public async Task DeleteMemberFromEvent(DeleteAndAddMemberRequestDTO requestDTO)
    {
        var result = deleteAndAddValidator.Validate(requestDTO);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        await unitOfWork.MemberRepository.RemoveFromEvent(requestDTO.MemberId, requestDTO.EventId);
    }

    public async Task AddMemberToEvent(DeleteAndAddMemberRequestDTO requestDTO)
    {
        var result = deleteAndAddValidator.Validate(requestDTO);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        await unitOfWork.MemberRepository.AddToEvent(requestDTO.MemberId, requestDTO.EventId);
    }

    public async Task UpdateMemberInformation(UpdateMemberDTO requestDTO)
    {

        var user = await unitOfWork.MemberRepository.GetById(requestDTO.Id);

        if (user == null)
        {
            throw new ItemNotFoundException("User");
        }

        user = UpdateUser(user, requestDTO);

        await unitOfWork.MemberRepository.Update(user);
    }

    private User UpdateUser(User user, UpdateMemberDTO requestDTO)
    {
        user.Birthday = requestDTO.Birthday == default ? user.Birthday : requestDTO.Birthday;
        user.FirstName = string.IsNullOrEmpty(requestDTO.FirstName) ? user.FirstName : requestDTO.FirstName;
        user.LastName = string.IsNullOrEmpty(requestDTO.LastName) ? user.LastName : requestDTO.LastName;


        return user;
    }

    private IEnumerable<GetMemberDTO> MapMembers(IEnumerable<User> users)
    {
        return users.Select(MapMember);
    }

    private GetMemberDTO MapMember(User user)
    {
        var userDTO = mapper.Map<GetMemberDTO>(user);
        return userDTO;
    }


}
