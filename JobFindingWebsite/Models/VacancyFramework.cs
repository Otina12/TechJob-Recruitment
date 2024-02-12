namespace JobFindingWebsite.Models
{
    public class VacancyFramework
    {
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public int FrameworkId { get; set; }
        public Framework Framework { get; set; }
    }
}
