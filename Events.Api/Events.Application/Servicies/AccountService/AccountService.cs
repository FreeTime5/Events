using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Servicies.AccountService;

public class AccountService
{
    private readonly UserManager<Member> _userManger;

    public AccountService(UserManager<Member> userManger)
    {
        _userManger = userManger;
    }
}
