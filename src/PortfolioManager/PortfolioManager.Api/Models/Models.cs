using System.Text.Json;

namespace PortfolioManager.Api.Models;

public record TokenGenerationRequest(string Username, string Password);

public record UserRegistrationModel
{
    //[Required(ErrorMessage = "Username is required")]
    //[StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
    public required string Username { get; set; }

    //[Required(ErrorMessage = "Password is required")]
    //[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public required string Password { get; set; }

    public bool IsAdmin { get; set; }
}