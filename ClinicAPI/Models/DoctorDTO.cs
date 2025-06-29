using System.ComponentModel.DataAnnotations;

public class DoctorDto
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    // ✅ This is used only for GET (display), NOT required in POST
    public string? DepartmentName { get; set; }
}
