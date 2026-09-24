using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompassionConnect.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
    }

    // ----------------------------------------------------------------- Register
    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!model.AcceptTerms)
        {
            ModelState.AddModelError(nameof(model.AcceptTerms), "You must agree to the terms to register.");
        }

        if (!ModelState.IsValid) return View(model);

        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing is not null)
        {
            ModelState.AddModelError(string.Empty, "An account with this email already exists.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            EmailConfirmed = true,
            DateRegistered = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "Donor");

        await _unitOfWork.Donors.AddAsync(new Donor
        {
            ApplicationUserId = user.Id,
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            DateCreated = DateTime.UtcNow
        });
        await _unitOfWork.CompleteAsync();

        await _signInManager.SignInAsync(user, isPersistent: false);
        TempData["Success"] = "Welcome to Gift of the Givers. Your donor account has been created.";
        return RedirectToAction("Dashboard", "Donor");
    }

    // ------------------------------------------------------------- Donor login
    [HttpGet]
    public IActionResult LoginDonor(string? returnUrl = null) =>
        View(new DonorLoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginDonor(DonorLoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null || !await _userManager.IsInRoleAsync(user, "Donor"))
        {
            ModelState.AddModelError(string.Empty, "Invalid donor email or password.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid donor email or password.");
            return View(model);
        }

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Dashboard", "Donor");
    }

    // --------------------------------------------------------- Employee login
    [HttpGet]
    public IActionResult LoginEmployee(string? returnUrl = null) =>
        View(new EmployeeLoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginEmployee(EmployeeLoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null || !await _userManager.IsInRoleAsync(user, "Employee"))
        {
            ModelState.AddModelError(string.Empty, "Invalid employee email or password.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid employee email or password.");
            return View(model);
        }

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Dashboard", "Employee");
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View(new ForgotPasswordViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // In production this would email a reset token via IEmailSender.
        // A user-existence-neutral message is shown regardless, to avoid account enumeration.
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is not null)
        {
            _ = await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        TempData["Success"] = "If an account exists for that email, password reset instructions have been sent.";
        return RedirectToAction(nameof(ForgotPassword));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();

    // ------------------------------------------------------------------ Profile
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return RedirectToAction(nameof(LoginDonor));

        var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");
        var model = new ProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            Role = isEmployee ? "Employee" : "Donor",
            MemberSince = user.DateRegistered
        };

        if (!isEmployee)
        {
            var donor = await _unitOfWork.Donors.GetByApplicationUserIdAsync(user.Id);
            if (donor is not null)
            {
                model.Address = donor.Address;
                model.City = donor.City;
                model.Province = donor.Province;
                model.PostalCode = donor.PostalCode;
            }
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return RedirectToAction(nameof(LoginDonor));

        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        await _userManager.UpdateAsync(user);

        var donor = await _unitOfWork.Donors.GetByApplicationUserIdAsync(user.Id);
        if (donor is not null)
        {
            donor.FullName = model.FullName;
            donor.PhoneNumber = model.PhoneNumber;
            donor.Address = model.Address;
            donor.City = model.City;
            donor.Province = model.Province;
            donor.PostalCode = model.PostalCode;
            _unitOfWork.Donors.Update(donor);
            await _unitOfWork.CompleteAsync();
        }

        TempData["Success"] = "Your profile has been updated.";
        return RedirectToAction(nameof(Profile));
    }

    // ----------------------------------------------------------------- Settings
    [Authorize]
    [HttpGet]
    public IActionResult Settings() => View(new ChangePasswordViewModel());

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return RedirectToAction(nameof(LoginDonor));

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["Success"] = "Your password has been changed.";
        return RedirectToAction(nameof(Settings));
    }
}
