using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.Services.Interfaces;
using CompassionConnect.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompassionConnect.Web.Controllers;

[Authorize(Roles = "Donor")]
public class DonorController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDonationService _donationService;
    private readonly ITaxCertificateService _taxCertificateService;

    public DonorController(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IDonationService donationService,
        ITaxCertificateService taxCertificateService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _donationService = donationService;
        _taxCertificateService = taxCertificateService;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var donor = await _unitOfWork.Donors.GetByApplicationUserIdAsync(user.Id);
        if (donor is null) return NotFound();

        var donations = await _donationService.GetDonorHistoryAsync(donor.DonorId);
        var certificates = await _taxCertificateService.GetByDonorIdAsync(donor.DonorId);

        var model = new DonorDashboardViewModel
        {
            Donor = donor,
            Donations = donations,
            TotalDonated = donations.Where(d => d.Status == DonationStatus.Completed).Sum(d => d.Amount),
            CertificatesIssued = certificates.Count
        };

        return View(model);
    }

    public async Task<IActionResult> History()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var donor = await _unitOfWork.Donors.GetByApplicationUserIdAsync(user.Id);
        if (donor is null) return NotFound();

        var donations = await _donationService.GetDonorHistoryAsync(donor.DonorId);
        return View(donations);
    }

    public async Task<IActionResult> TaxCertificates()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var donor = await _unitOfWork.Donors.GetByApplicationUserIdAsync(user.Id);
        if (donor is null) return NotFound();

        var certificates = await _taxCertificateService.GetByDonorIdAsync(donor.DonorId);
        return View(certificates);
    }

    public async Task<IActionResult> DownloadCertificate(string certificateNumber)
    {
        var certificate = await _taxCertificateService.GetByCertificateNumberAsync(certificateNumber);
        if (certificate is null) return NotFound();

        var pdfBytes = await _taxCertificateService.GeneratePdfAsync(certificate);
        return File(pdfBytes, "application/pdf", $"{certificate.CertificateNumber}.pdf");
    }
}
