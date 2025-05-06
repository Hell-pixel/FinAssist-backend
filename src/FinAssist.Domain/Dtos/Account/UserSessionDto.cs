namespace FinAssist.Domain.Dtos.Account;

public class UserSessionDto
{
    public Guid Id { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
}