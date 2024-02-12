using JobFindingWebsite.Models;
using JobFindingWebsite.Data.Enum;
using Azure.Core;

namespace JobFindingWebsite.ViewModels
{
    public class VacancyAnalyticsViewModel
    {
        public Vacancy Vacancy { get; set; } = null!;
        public List<(AppUser, StatusType)>? Applicants { get; set; }
    }
}
