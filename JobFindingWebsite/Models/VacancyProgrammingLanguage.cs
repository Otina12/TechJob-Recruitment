namespace JobFindingWebsite.Models
{
    public class VacancyProgrammingLanguage
    {
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}
