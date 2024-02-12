using JobFindingWebsite.Extensions;

namespace JobFindingWebsite.Data.Enum
{
    public enum HoursType
    {
        [DisplayName("Full-Time")]
        Fulltime,
        [DisplayName("Part-Time")]
        Parttime,
        Flexible,
        Contract
    }
}
