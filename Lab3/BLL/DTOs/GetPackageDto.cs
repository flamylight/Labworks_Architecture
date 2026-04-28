namespace BLL.DTOs;

public class GetPackageDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    
    public List<PackageServiceItemDto> PackageServicesItemDto { get; set; } = new();
}