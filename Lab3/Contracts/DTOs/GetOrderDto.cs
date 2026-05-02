namespace Contracts.DTOs;

public class GetOrderDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public DateTime? FinishedAt { get; set; }
    public bool IsDone { get; set; }
    public bool IsInPortfolio { get; set; }
    public string ClientDescription { get; set; }  = String.Empty;
    public bool IsTurnkey { get; set; }
    public List<OrderServiceItemDto> OrderServices { get; set; } = new();
}