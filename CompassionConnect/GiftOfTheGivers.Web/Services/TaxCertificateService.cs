using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using GiftOfTheGivers.Web.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GiftOfTheGivers.Web.Services;

/// <summary>Issues Section 18A tax certificates and renders them as printable PDFs.</summary>
public class TaxCertificateService : ITaxCertificateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReferenceNumberService _referenceNumberService;

    public TaxCertificateService(IUnitOfWork unitOfWork, IReferenceNumberService referenceNumberService)
    {
        _unitOfWork = unitOfWork;
        _referenceNumberService = referenceNumberService;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<TaxCertificate> IssueForDonationAsync(int donationId)
    {
        var existing = await _unitOfWork.TaxCertificates.GetByDonationIdAsync(donationId);
        if (existing is not null) return existing;

        var donation = await _unitOfWork.Donations.GetByIdAsync(donationId)
            ?? throw new InvalidOperationException($"Donation {donationId} not found.");

        var certificate = new TaxCertificate
        {
            CertificateNumber = await _referenceNumberService.GenerateTaxCertificateNumberAsync(),
            DonationId = donation.DonationId,
            DonorId = donation.DonorId,
            Amount = donation.Amount,
            DateIssued = DateTime.UtcNow
        };

        await _unitOfWork.TaxCertificates.AddAsync(certificate);
        await _unitOfWork.CompleteAsync();
        return certificate;
    }

    public async Task<TaxCertificate?> GetByCertificateNumberAsync(string certificateNumber) =>
        await _unitOfWork.TaxCertificates.GetByCertificateNumberAsync(certificateNumber);

    public async Task<IReadOnlyList<TaxCertificate>> GetByDonorIdAsync(int donorId) =>
        await _unitOfWork.TaxCertificates.GetByDonorIdAsync(donorId);

    public Task<byte[]> GeneratePdfAsync(TaxCertificate certificate)
    {
        var donorName = certificate.Donor?.FullName ?? "Valued Donor";
        var projectTitle = certificate.Donation?.Project?.Title ?? "General Emergency Fund";

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Helvetica"));

                page.Header().Column(col =>
                {
                    col.Item().Text("Gift of the Givers Foundation").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text("Section 18A Tax Deductible Donation Certificate").FontSize(12).FontColor(Colors.Grey.Darken1);
                    col.Item().PaddingTop(4).LineHorizontal(1).LineColor(Colors.Blue.Darken2);
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text($"Certificate Number: {certificate.CertificateNumber}").Bold();
                    col.Item().Text($"Date Issued: {certificate.DateIssued:dd MMMM yyyy}");
                    col.Item().PaddingTop(10).Text("This is to certify that a donation was received from:");
                    col.Item().Text(donorName).FontSize(14).Bold();
                    col.Item().PaddingTop(10).Text($"Donation Reference: {certificate.Donation?.ReferenceNumber}");
                    col.Item().Text($"Amount: {certificate.Donation?.Currency} {certificate.Amount:N2}").FontSize(14).Bold();
                    col.Item().Text($"Designated Project: {projectTitle}");
                    col.Item().PaddingTop(20).Text(
                        "This donation qualifies for a tax deduction in terms of Section 18A of the " +
                        "Income Tax Act, No. 58 of 1962, as the Gift of the Givers Foundation is a " +
                        "registered Public Benefit Organisation. No goods or services were provided " +
                        "in exchange for this donation.");
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Gift of the Givers Foundation  |  PBO Registration Number: 930033254  |  NPO Number: 015-644").FontSize(8).FontColor(Colors.Grey.Darken1);
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }
}


