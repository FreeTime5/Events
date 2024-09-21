using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Entities;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public Category Category { get; set; }

    public int MaxMembers { get; set; }

    public List<Registration> Registrations { get; set; } = [];

    public Image EventImage { get; set; }

    public bool HasImage { get; set; } 
}
