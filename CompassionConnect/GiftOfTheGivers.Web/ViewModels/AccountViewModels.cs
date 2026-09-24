using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.ViewModels;

public class DonorLoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class EmployeeLoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required, MaxLength(150), Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone, Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password)), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "I agree to the terms and privacy policy")]
    public bool AcceptTerms { get; set; }
}

public class ForgotPasswordViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ProfileViewModel
{
    [Required, MaxLength(150), Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone, Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }

    [Display(Name = "Postal code")]
    public string? PostalCode { get; set; }

    public string Role { get; set; } = "Donor";
    public DateTime MemberSince { get; set; }
}

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), Display(Name = "Current password")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8), Display(Name = "New password")]
    public string NewPassword { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(NewPassword)), Display(Name = "Confirm new password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}


