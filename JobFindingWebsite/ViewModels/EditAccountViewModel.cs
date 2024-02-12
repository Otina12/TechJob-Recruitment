namespace JobFindingWebsite.ViewModels
{
    public class EditAccountViewModel
    {
        public EditUserViewModel? User { get; set; }
        public EditCompanyViewModel? Company { get; set; }
        public string UserName { get; set; }
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? LinkedInURL { get; set; }
    }
}
