using Microsoft.AspNetCore.Mvc;

namespace PortfolioManager.Application.Contracts;

public record CreateTokenRequest(string Username, string Password);
public record CreateUserRequest(string Username, string Password, bool IsAdmin);
public record GetAllInvestmentsRequest([FromQuery]int PageIndex = 1, [FromQuery]int PageSize = 10);