﻿namespace PortfolioManager.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid(); 
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsAdmin { get; set; } = false;
}
