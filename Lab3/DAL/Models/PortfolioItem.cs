namespace DAL.Models;

public class PortfolioItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
}