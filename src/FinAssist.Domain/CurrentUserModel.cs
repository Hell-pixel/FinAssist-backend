namespace FinAssist.Domain;

public class CurrentUserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public Guid CurrentSessionId { get; set; }
}