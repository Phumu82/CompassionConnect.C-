using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<ReliefProject> ReliefProjects => Set<ReliefProject>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<TaxCertificate> TaxCertificates => Set<TaxCertificate>();
    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Donor>()
            .HasIndex(d => d.Email);

        builder.Entity<Donation>()
            .HasIndex(d => d.ReferenceNumber)
            .IsUnique();

        builder.Entity<Donation>()
            .HasOne(d => d.Donor)
            .WithMany(donor => donor.Donations)
            .HasForeignKey(d => d.DonorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Donation>()
            .HasOne(d => d.Project)
            .WithMany(p => p.Donations)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Donation>()
            .Property(d => d.Type)
            .HasConversion<string>();

        builder.Entity<Donation>()
            .Property(d => d.Status)
            .HasConversion<string>();

        builder.Entity<Donation>()
            .Property(d => d.PaymentMethod)
            .HasConversion<string>();

        builder.Entity<TaxCertificate>()
            .HasIndex(t => t.CertificateNumber)
            .IsUnique();

        builder.Entity<TaxCertificate>()
            .HasOne(t => t.Donation)
            .WithOne(d => d.TaxCertificate)
            .HasForeignKey<TaxCertificate>(t => t.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TaxCertificate>()
            .HasOne(t => t.Donor)
            .WithMany()
            .HasForeignKey(t => t.DonorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Volunteer>()
            .HasIndex(v => v.ReferenceNumber)
            .IsUnique();

        builder.Entity<Volunteer>()
            .Property(v => v.Status)
            .HasConversion<string>();

        builder.Entity<Volunteer>()
            .Property(v => v.Availability)
            .HasConversion<string>();

        builder.Entity<Volunteer>()
            .HasOne(v => v.Project)
            .WithMany(p => p.Volunteers)
            .HasForeignKey(v => v.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<ReliefProject>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder.Entity<ReliefProject>()
            .Property(p => p.Status)
            .HasConversion<string>();

        builder.Entity<ReliefProject>()
            .Property(p => p.Category)
            .HasConversion<string>();

        builder.Entity<ReliefProject>()
            .HasOne(p => p.ProjectLead)
            .WithMany(e => e.ManagedProjects)
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<NewsArticle>()
            .HasIndex(n => n.Slug)
            .IsUnique();

        builder.Entity<NewsArticle>()
            .Property(n => n.Category)
            .HasConversion<string>();

        builder.Entity<Employee>()
            .HasIndex(e => e.EmployeeNumber)
            .IsUnique();
    }
}


