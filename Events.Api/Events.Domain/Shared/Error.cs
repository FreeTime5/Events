using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Shared;

public class Error
{
    public string Message { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public Error(string message, string code, string name)
    {
        Message = message;
        Code = code;
        Name = name;
    }
}
