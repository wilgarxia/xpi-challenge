namespace PortfolioManager.Infrastructure.Email;

public class EmailSettings
{
    public const string Section = "EmailSettings";

    public string DefaultFromEmail { get; init; } = null!;

    public SmtpSettings SmtpSettings { get; init; } = null!;
}