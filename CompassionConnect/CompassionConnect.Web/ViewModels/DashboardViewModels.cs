using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.ViewModels;

public class EmployeeDashboardViewModel
{
    public decimal TotalRaised { get; set; }
    public int TotalDonations { get; set; }
    public int ActiveVolunteers { get; set; }
    public int ActiveProjects { get; set; }
    public IReadOnlyList<Donation> RecentDonations { get; set; } = new List<Donation>();
    public IReadOnlyList<Volunteer> RecentVolunteers { get; set; } = new List<Volunteer>();
    public IReadOnlyList<ReliefProject> Projects { get; set; } = new List<ReliefProject>();
}

public class DonorDashboardViewModel
{
    public Donor? Donor { get; set; }
    public IReadOnlyList<Donation> Donations { get; set; } = new List<Donation>();
    public decimal TotalDonated { get; set; }
    public int CertificatesIssued { get; set; }
}

public class HomeViewModel
{
    public IReadOnlyList<ReliefProject> EmergencyCampaigns { get; set; } = new List<ReliefProject>();
    public IReadOnlyList<NewsArticle> LatestNews { get; set; } = new List<NewsArticle>();
    public decimal TotalRaised { get; set; }
    public int PeopleAssisted { get; set; }
    public int ActiveVolunteers { get; set; }
    public int ActiveProjects { get; set; }
}
