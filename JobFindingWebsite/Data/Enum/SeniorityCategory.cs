using JobFindingWebsite.Extensions;

namespace JobFindingWebsite.Data.Enum
{
    public enum SeniorityCategory
    {
        Junior,
        Middle,
        Senior,
        Internship,
        [DisplayName("Not Applicable")]
        NotApplicable
    }
}
