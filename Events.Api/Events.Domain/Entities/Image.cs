﻿namespace Events.Domain.Entities;

public class Image
{
    public Guid Id { get; set; }

    public string Url { get; set; } = string.Empty;
}
