namespace BLL.DTOs;

public class CreateOrderDto
{
    public string Title { get; set; } = string.Empty;
    public string ClientDescription { get; set; } = string.Empty;
    public bool IsTurnkey { get; set; }
    public Guid? PackageId { get; set; }  
    public List<Guid> ServiceIds { get; set; } = new();
}