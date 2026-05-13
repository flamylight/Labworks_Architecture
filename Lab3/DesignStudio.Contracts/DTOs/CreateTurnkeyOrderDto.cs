namespace Contracts.DTOs;

public class CreateTurnkeyOrderDto
{
    public string Title { get; set; } = string.Empty;
    public string ClientDescription { get; set; } = string.Empty;
    public Guid? PackageId { get; set; }
}