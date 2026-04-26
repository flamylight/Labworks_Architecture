namespace DAL.Models;

public class Service
{
    public Guid Id { get; set; }= Guid.NewGuid();
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
}