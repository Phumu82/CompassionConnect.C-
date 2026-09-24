using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Web.Models;

/// <summary>
/// Extends ASP.NET Identity's IdentityUser with fields shared by both
/// Donor and Employee accounts. Role-specific data lives in the
/// related <see cref="Donor"/> or <see cref="Employee"/> profile.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public Donor? DonorProfile { get; set; }
    public Employee? EmployeeProfile { get; set; }
}


