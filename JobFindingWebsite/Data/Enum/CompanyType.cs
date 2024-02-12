using JobFindingWebsite.Extensions;

namespace JobFindingWebsite.Data.Enum
{
    public enum CompanyType
    {
        Technology,
        Bank,
        Healthcare,
        Retail,
        [DisplayName("E-Commerce")]
        ECommerce,
        Manufacturing,
        Finance,
        Automotive,
        Energy,
        Telecommunications,
        Education,
        Media,
        Entertainment,
        Government,
        Consulting,
        Logistics,
        RealEstate,
        Agriculture,
        Pharmaceutical,
        Legal,
        NonProfit,
        [DisplayName("System Administrator")]
        FoodAndBeverage
    }
}