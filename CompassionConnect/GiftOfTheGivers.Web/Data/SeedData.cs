using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Data;

/// <summary>
/// Seeds Identity roles, demo accounts, relief projects and news articles.
/// </summary>
public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // ============================================================
        // ROLES
        // ============================================================

        foreach (var role in new[] { "Employee", "Donor" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // ============================================================
        // RELIEF PROJECTS / CAMPAIGNS
        // ============================================================

        if (!await context.ReliefProjects.AnyAsync())
        {
            var projects = new List<ReliefProject>
            {
                // ----------------------------------------------------
                // 1. FLOOD RELIEF
                // ----------------------------------------------------
                new()
                {
                    Title = "KwaZulu-Natal Flood Relief",
                    Slug = "kwazulu-natal-flood-relief",
                    Province = "KwaZulu-Natal",
                    Category = ProjectCategory.FloodRelief,
                    Status = ProjectStatus.Active,
                    Summary = "Emergency water, food and shelter for communities cut off by flooding along the Mgeni river.",
                    Description = "Twelve days after the Mgeni river burst its banks, relief convoys continue to reach isolated communities with water, food parcels, blankets and hygiene kits. Distribution continues on a fortnightly cycle until municipal water reticulation is restored.",
                    GoalAmount = 8500000,
                    RaisedAmount = 5230000,
                    PeopleAssisted = 46000,
                    ImageUrl = "/images/news/campaign-flood.jpg",
                    IsEmergency = true,
                    StartDate = DateTime.UtcNow.AddDays(-40)
                },

                // ----------------------------------------------------
                // 2. WILDFIRE
                // ----------------------------------------------------
                new()
                {
                    Title = "Western Cape Wildfire Support",
                    Slug = "western-cape-wildfire-support",
                    Province = "Western Cape",
                    Category = ProjectCategory.WildfireResponse,
                    Status = ProjectStatus.Active,
                    Summary = "Rebuilding support, trauma counselling and livestock feed for Overberg farms after the wildfire.",
                    Description = "With the fire line contained, teams have shifted from emergency supply to rebuilding support, distributing livestock feed and building material vouchers, and running weekly trauma debriefing sessions for residents and volunteer firefighters.",
                    GoalAmount = 4200000,
                    RaisedAmount = 2870000,
                    PeopleAssisted = 9600,
                    ImageUrl = "/images/news/campaign-wildfire.jpg",
                    IsEmergency = true,
                    StartDate = DateTime.UtcNow.AddDays(-65)
                },

                // ----------------------------------------------------
                // 3. MEDICAL AID
                // ----------------------------------------------------
                new()
                {
                    Title = "Rural Mobile Medical Clinics",
                    Slug = "rural-mobile-medical-clinics",
                    Province = "Eastern Cape",
                    Category = ProjectCategory.MedicalAid,
                    Status = ProjectStatus.Active,
                    Summary = "A growing fleet of mobile clinics bringing primary healthcare to remote villages.",
                    Description = "Four mobile clinics now run fixed weekly routes through remote villages in OR Tambo District, offering chronic medication refills, maternal check-ups, screening and childhood immunisation, cutting average travel time to a clinic from four hours to under forty minutes.",
                    GoalAmount = 6000000,
                    RaisedAmount = 3980000,
                    PeopleAssisted = 21500,
                    ImageUrl = "/images/news/campaign-medical.jpg",
                    IsEmergency = false,
                    StartDate = DateTime.UtcNow.AddDays(-220)
                },

                // ----------------------------------------------------
                // 4. FOOD DISTRIBUTION
                // ----------------------------------------------------
                new()
                {
                    Title = "National Food Distribution",
                    Slug = "national-food-distribution",
                    Province = "Gauteng",
                    Category = ProjectCategory.FoodDistribution,
                    Status = ProjectStatus.Active,
                    Summary = "Monthly food parcels for vulnerable households through a nationwide warehouse network.",
                    Description = "A donated 1,800m² Ekurhuleni facility doubled monthly packing capacity, with 9,000 food parcels now packed and distributed across Gauteng every month by registered volunteers.",
                    GoalAmount = 12000000,
                    RaisedAmount = 9100000,
                    PeopleAssisted = 118000,

                    // FIXED: was incorrectly using medical image
                    ImageUrl = "/images/news/news-warehouse.jpg",

                    IsEmergency = false,
                    StartDate = DateTime.UtcNow.AddDays(-300)
                },

                // ----------------------------------------------------
                // 5. WATER / BOREHOLE
                // ----------------------------------------------------
                new()
                {
                    Title = "Limpopo Borehole Project",
                    Slug = "limpopo-borehole-project",
                    Province = "Limpopo",
                    Category = ProjectCategory.WaterAndSanitation,
                    Status = ProjectStatus.Active,
                    Summary = "Drilling boreholes and solar pumps for villages across the Vhembe District.",
                    Description = "Hydrogeological surveys confirmed twenty-one of twenty-four proposed sites in the Vhembe District as viable for drilling. Each site receives a borehole, a solar pump and a communal standpipe, with a trained village water committee, serving roughly 14,000 residents once complete.",
                    GoalAmount = 3500000,
                    RaisedAmount = 1450000,
                    PeopleAssisted = 14000,

                    // FIXED: was incorrectly using flood image
                    ImageUrl = "/images/news/news-borehole.jpg",

                    IsEmergency = false,
                    StartDate = DateTime.UtcNow.AddDays(-90)
                },

                // ----------------------------------------------------
                // 6. GENERAL EMERGENCY FUND
                // ----------------------------------------------------
                new()
                {
                    Title = "General Emergency Fund",
                    Slug = "general-emergency-fund",
                    Province = "National",
                    Category = ProjectCategory.GeneralEmergencyFund,
                    Status = ProjectStatus.Active,
                    Summary = "Unrestricted funding held ready to deploy within 12 hours of any verified disaster.",
                    Description = "The General Emergency Fund keeps pre-positioned relief stock in four provincial warehouses so a first response convoy can leave within twelve hours of a verified disaster alert, anywhere in South Africa.",
                    GoalAmount = 10000000,
                    RaisedAmount = 6200000,
                    PeopleAssisted = 60000,

                    // FIXED: was incorrectly using wildfire image
                    ImageUrl = "/images/news/hero-relief.jpg",

                    IsEmergency = false,
                    StartDate = DateTime.UtcNow.AddDays(-500)
                }
            };

            await context.ReliefProjects.AddRangeAsync(projects);
            await context.SaveChangesAsync();
        }

        // ============================================================
        // NEWS ARTICLES
        // ============================================================

        var allArticles = new List<NewsArticle>
        {
            // --------------------------------------------------------
            // 1. FLOOD NEWS
            // --------------------------------------------------------
            new()
            {
                Title = "Relief convoys reach the last flood cut-off communities in KwaZulu-Natal",
                Slug = "relief-convoys-reach-last-flood-cutoff-communities",
                Category = NewsCategory.DisasterResponse,
                Author = "Operations Desk",
                ReadMinutes = 5,
                Excerpt = "After twelve days of road clearing, convoys reached the last three communities isolated by the flooding, delivering water, food parcels and medical supplies to 1,900 households.",
                Body = "Twelve days after the Mgeni river burst its banks, the final three cut-off settlements above Ndwedwe were reached by road. Engineering crews cleared four collapsed culverts and laid temporary steel decking to carry the convoy weight.\n\nEach household received a 20 litre water container, a 15 kg food parcel, blankets and a hygiene kit. Mobile registration teams captured beneficiary details on tablets so repeat distributions can be tracked against the same household record.\n\nDistribution continues on a fortnightly cycle until municipal water reticulation is restored, currently projected for late September.",

                // FIXED: flood article should use flood image
                ImageUrl = "/images/news/campaign-flood.jpg",

                ImageAlt = "Relief workers delivering emergency supplies to a flood-affected community",
                Featured = true,
                PublishedDate = new DateTime(2026, 7, 30)
            },

            // --------------------------------------------------------
            // 2. MOBILE CLINIC NEWS
            // --------------------------------------------------------
            new()
            {
                Title = "Fourth mobile clinic commissioned for OR Tambo District",
                Slug = "fourth-mobile-clinic-or-tambo-district",
                Category = NewsCategory.Healthcare,
                Author = "Dr. Aisha Khan",
                ReadMinutes = 4,
                Excerpt = "The new vehicle extends primary healthcare coverage to seven additional villages, cutting average travel time to a clinic from four hours to under forty minutes.",
                Body = "The fourth mobile clinic in the Eastern Cape fleet was handed over this week, fitted with a consulting bay, a small pharmacy store and a solar cold chain for vaccines.\n\nTwo professional nurses and a visiting clinician will run a fixed weekly route through seven villages, offering chronic medication refills, maternal check-ups, HIV and TB screening and childhood immunisation.\n\nAverage travel time to a health facility across the served area drops from roughly four hours to under forty minutes.",

                // FIXED: was using volunteer training image
                ImageUrl = "/images/news/campaign-medical.jpg",

                ImageAlt = "Mobile medical clinic providing healthcare to patients in a rural community",
                PublishedDate = new DateTime(2026, 7, 24)
            },

            // --------------------------------------------------------
            // 3. WILDFIRE NEWS
            // --------------------------------------------------------
            new()
            {
                Title = "Overberg wildfire response enters recovery phase",
                Slug = "overberg-wildfire-response-recovery-phase",
                Category = NewsCategory.DisasterResponse,
                Author = "Field Report",
                ReadMinutes = 6,
                Excerpt = "With the fire line contained, teams have shifted from emergency supply to rebuilding support, trauma counselling and livestock feed distribution for affected farms.",
                Body = "The fire line across the Overberg was declared contained after nine days. Emergency feeding of displaced families has now wound down and the operation has moved into recovery.\n\nTeams are distributing livestock feed to smallholder farms that lost grazing, and supplying building material vouchers to 74 households whose homes were damaged.\n\nTwo counsellors are running weekly trauma debriefing sessions in Caledon and Napier for residents and volunteer firefighters.",
                ImageUrl = "/images/news/campaign-wildfire.jpg",
                ImageAlt = "Firefighters and relief workers responding to wildfire damage",
                PublishedDate = new DateTime(2026, 7, 16)
            },

            // --------------------------------------------------------
            // 4. IMPACT REPORT
            // --------------------------------------------------------
            new()
            {
                Title = "Quarterly impact report: where every rand went",
                Slug = "quarterly-impact-report-where-every-rand-went",
                Category = NewsCategory.Reports,
                Author = "Finance Office",
                ReadMinutes = 8,
                Excerpt = "Our Q2 report details programme spend across all 316 active projects, with 92.4 cents in every rand delivered directly to beneficiaries.",
                Body = "The Q2 financial review was tabled this week and independently reviewed. Of every rand received, 92.4 cents reached programme delivery, 4.9 cents covered logistics overhead and 2.7 cents covered administration.\n\nSpend is broken down by province, category and project, with donation references traceable end-to-end from receipt through to distribution manifest.\n\nThe full report, including the auditor's letter, is available on request from the finance office.",

                ImageUrl = "/images/news/news-impact-report.jpg",

                ImageAlt = "Foundation finance staff reviewing impact reports and financial information",
                PublishedDate = new DateTime(2026, 7, 8)
            },

            // --------------------------------------------------------
            // 5. PARTNERSHIP
            // --------------------------------------------------------
            new()
            {
                Title = "Meridian Bank renews three-year disaster readiness partnership",
                Slug = "meridian-bank-renews-disaster-readiness-partnership",
                Category = NewsCategory.Partnerships,
                Author = "Partnerships Team",
                ReadMinutes = 3,
                Excerpt = "The renewed agreement funds pre-positioned relief stock in four provincial warehouses, cutting deployment time for new emergencies.",
                Body = "Meridian Bank has renewed its disaster readiness partnership for a further three years, underwriting the cost of pre-positioned relief stock.\n\nThe funding keeps four provincial warehouses stocked with water, tarpaulins, blankets and ready-to-eat rations so that a first convoy can leave within twelve hours of a verified alert.\n\nThe agreement also supports an annual staff volunteering programme with bank employees joining packing shifts.",

                // FIXED: partnership now uses partnership image
                ImageUrl = "/images/news/news-partnership.jpg",

                ImageAlt = "Corporate partnership supporting humanitarian disaster readiness",
                PublishedDate = new DateTime(2026, 6, 27)
            },

            // --------------------------------------------------------
            // 6. VOLUNTEER TRAINING
            // --------------------------------------------------------
            new()
            {
                Title = "Eight hundred volunteers complete disaster readiness training",
                Slug = "eight-hundred-volunteers-complete-readiness-training",
                Category = NewsCategory.Community,
                Author = "Volunteer Office",
                ReadMinutes = 4,
                Excerpt = "The national training cycle certified 812 volunteers in first aid, water purification, shelter assembly and beneficiary registration.",
                Body = "The 2026 national training cycle closed with 812 volunteers certified across all nine provinces.\n\nThe curriculum covers first aid, safe water treatment, emergency shelter assembly, and digital beneficiary registration on the field tablet system.\n\nCertified volunteers enter the standby roster and can be called up by province and skill when an emergency is declared.",
                ImageUrl = "/images/news/news-volunteer-training.jpg",
                ImageAlt = "Volunteers completing disaster readiness and emergency shelter training",
                PublishedDate = new DateTime(2026, 6, 15)
            },

            // --------------------------------------------------------
            // 7. WAREHOUSE
            // --------------------------------------------------------
            new()
            {
                Title = "Warehouse donation doubles food parcel packing capacity",
                Slug = "warehouse-donation-doubles-food-parcel-packing-capacity",
                Category = NewsCategory.Community,
                Author = "Logistics Desk",
                ReadMinutes = 3,
                Excerpt = "A donated 1,800m² facility in Ekurhuleni now allows the packing of 9,000 food parcels per month for Gauteng distribution.",
                Body = "A donated 1,800m² facility in Ekurhuleni came online this month, doubling monthly packing capacity to 9,000 food parcels.\n\nThe site includes racking for palletised stock, a dedicated packing floor and a loading bay that allows two trucks to be turned around simultaneously.\n\nPacking shifts run three days a week and are open to registered volunteers in Gauteng.",
                ImageUrl = "/images/news/news-warehouse.jpg",
                ImageAlt = "Volunteers packing food parcels inside a humanitarian aid warehouse",
                PublishedDate = new DateTime(2026, 6, 2)
            },

            // --------------------------------------------------------
            // 8. BOREHOLE
            // --------------------------------------------------------
            new()
            {
                Title = "Hydrogeological surveys clear the way for Limpopo boreholes",
                Slug = "hydrogeological-surveys-limpopo-boreholes",
                Category = NewsCategory.Reports,
                Author = "Engineering Unit",
                ReadMinutes = 5,
                Excerpt = "Twenty-one of twenty-four proposed borehole sites in the Vhembe District were confirmed viable, with drilling scheduled to begin in September.",
                Body = "Survey teams completed hydrogeological assessments at twenty-four proposed sites across the Vhembe District, confirming twenty-one as viable for drilling.\n\nEach viable site will receive a borehole, a solar pump and a communal standpipe, with a village water committee trained on basic maintenance.\n\nDrilling begins in September and is expected to serve roughly 14,000 residents once complete.",

                // FIXED: was using flood image
                ImageUrl = "/images/news/news-borehole.jpg",

                ImageAlt = "Community members collecting clean water from a borehole",
                PublishedDate = new DateTime(2026, 5, 21)
            }
        };

        // ============================================================
        // ADD ONLY NEW NEWS ARTICLES
        // ============================================================

        var existingSlugs =
            (await context.NewsArticles
                .Select(n => n.Slug)
                .ToListAsync())
            .ToHashSet();

        var newArticles =
            allArticles
                .Where(a => !existingSlugs.Contains(a.Slug))
                .ToList();

        if (newArticles.Count > 0)
        {
            await context.NewsArticles.AddRangeAsync(newArticles);
            await context.SaveChangesAsync();
        }

        // ============================================================
        // DEMO EMPLOYEE ACCOUNT
        // ============================================================

        const string employeeEmail = "employee@giftofthegivers.org";

        if (await userManager.FindByEmailAsync(employeeEmail) is null)
        {
            var employeeUser = new ApplicationUser
            {
                UserName = employeeEmail,
                Email = employeeEmail,
                FullName = "Thabo Dlamini",
                EmailConfirmed = true,
                DateRegistered = DateTime.UtcNow
            };

            var result =
                await userManager.CreateAsync(
                    employeeUser,
                    "Employee@2026!"
                );

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    employeeUser,
                    "Employee"
                );

                context.Employees.Add(new Employee
                {
                    ApplicationUserId = employeeUser.Id,
                    FullName = employeeUser.FullName,
                    Email = employeeEmail,
                    JobTitle = "National Volunteer Manager",
                    Department = "Disaster Operations",
                    EmployeeNumber = "EMP-0001",
                    DateHired = DateTime.UtcNow.AddYears(-3)
                });

                await context.SaveChangesAsync();
            }
        }

        // ============================================================
        // DEMO DONOR ACCOUNT
        // ============================================================

        const string donorEmail = "donor@example.com";

        if (await userManager.FindByEmailAsync(donorEmail) is null)
        {
            var donorUser = new ApplicationUser
            {
                UserName = donorEmail,
                Email = donorEmail,
                FullName = "Thandi Nkosi",
                EmailConfirmed = true,
                DateRegistered = DateTime.UtcNow
            };

            var result =
                await userManager.CreateAsync(
                    donorUser,
                    "Donor@2026!"
                );

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    donorUser,
                    "Donor"
                );

                context.Donors.Add(new Donor
                {
                    ApplicationUserId = donorUser.Id,
                    FullName = donorUser.FullName,
                    Email = donorEmail,
                    PhoneNumber = "082 555 0134",
                    Province = "Gauteng",
                    City = "Johannesburg",
                    DateCreated = DateTime.UtcNow.AddMonths(-8)
                });

                await context.SaveChangesAsync();
            }
        }
    }
}

