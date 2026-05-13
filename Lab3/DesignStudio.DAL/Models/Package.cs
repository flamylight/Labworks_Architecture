namespace DAL.Models;

public class Package
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice => PackageServices.Sum(ps => ps.Service != null ? ps.Service.Price : 0);
    
    public List<PackageService> PackageServices { get; set; } = new();
}
