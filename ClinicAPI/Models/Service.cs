using ClinicAPI.Models;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    // Foreign key
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
