namespace DAL.Models;

public class OrderService
{
    public Guid OrderId { get; set; }
    public Guid ServiceId { get; set; }
    public Order? Order { get; set; }
    public Service? Service { get; set; }
}