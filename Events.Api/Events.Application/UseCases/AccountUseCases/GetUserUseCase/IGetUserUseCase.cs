using Events.Domain.Entities;

namespace Events.Application.UseCases.AccountUseCases.GetUserUseCase;

public interface IGetUserUseCase
{
    Task<Member?> Execute(string userName);
}
