namespace DAL.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = String.Empty;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
    public bool IsDone { get; set; }
    public bool IsInPortfolio { get; set; }
    public string ClientDescription { get; set; }  = String.Empty;
    public bool IsTurnkey { get; set; }
    public Guid? PackageId { get; set; }  
    public Package? Package { get; set; }
    public List<OrderService> OrderServices { get; set; } = new();
}