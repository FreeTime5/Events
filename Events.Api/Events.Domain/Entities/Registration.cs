using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Entities;

public class Registration
{
    public Member Member { get; set; }

    public Event Event { get; set; }

    public DateTime RegistrationDate { get; set; }
}
