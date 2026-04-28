namespace BLL.DTOs;

public class CreatePackageDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public List<Guid> Services { get; set; } = new();
}