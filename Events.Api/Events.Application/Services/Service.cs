using Events.Domain.Entities;
using Events.Infrastructure.UnitOfWork;
using System.Security.Claims;

namespace Events.Application.Services;

internal class Service
{
    protected readonly IUnitOfWork unitOfWork;

    public Service(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
}
