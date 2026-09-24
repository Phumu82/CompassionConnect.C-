using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompassionConnect.Web.Models;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }

    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string JobTitle { get; set; } = "Programme Officer";

    [MaxLength(100)]
    public string Department { get; set; } = "Disaster Operations";

    [MaxLength(20)]
    public string EmployeeNumber { get; set; } = string.Empty;

    public DateTime DateHired { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public ICollection<ReliefProject> ManagedProjects { get; set; } = new List<ReliefProject>();
}
