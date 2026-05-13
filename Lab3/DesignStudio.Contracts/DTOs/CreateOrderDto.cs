namespace Contracts.DTOs;

public class CreateOrderDto
{
    public string Title { get; set; } = string.Empty;
    public string ClientDescription { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
}