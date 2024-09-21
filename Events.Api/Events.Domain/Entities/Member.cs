using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Entities;

public class Member
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }

    public List<Registration> Registrations { get; set; } = [];

    public string Email { get; set; } = string.Empty;
}
