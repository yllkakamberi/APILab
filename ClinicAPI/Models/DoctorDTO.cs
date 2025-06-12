public class DoctorDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } // 👈 Add this
}
