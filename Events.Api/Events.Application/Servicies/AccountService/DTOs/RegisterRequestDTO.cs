﻿namespace Events.Application.Servicies.AccountService.DTOs;

public class RegisterRequestDTO
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
}
