using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobFindingWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Framework> Frameworks { get; set; }
        public DbSet<VacancyProgrammingLanguage> VacancyProgrammingLanguages { get; set; }
        public DbSet<VacancyFramework> VacancyFrameworks { get; set; }
        public DbSet<SavedVacancies> SavedVacancies { get; set; }
        public DbSet<AppliedVacancies> AppliedVacancies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<Vacancy>().ToTable("Vacancy");

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Vacancies)
                .WithOne(v => v.Company)
                .HasForeignKey(v => v.CompanyId);

            modelBuilder.Entity<AppliedVacancies>()
        .HasKey(av => new { av.VacancyId, av.AppUserId });

            modelBuilder.Entity<AppliedVacancies>()
                .HasOne(av => av.Vacancy)
                .WithMany(v => v.Applicants)
                .HasForeignKey(av => av.VacancyId);

            modelBuilder.Entity<AppliedVacancies>()
                .HasOne(av => av.AppUser)
                .WithMany(au => au.AppliedVacancies)
                .HasForeignKey(av => av.AppUserId);

            modelBuilder.Entity<SavedVacancies>()
                .HasKey(sv => new { sv.VacancyId, sv.AppUserId });

            modelBuilder.Entity<SavedVacancies>()
                .HasOne(sv => sv.Vacancy)
                .WithMany(v => v.Savers)
                .HasForeignKey(sv => sv.VacancyId);

            modelBuilder.Entity<SavedVacancies>()
                .HasOne(sv => sv.AppUser)
                .WithMany(au => au.SavedVacancies)
                .HasForeignKey(sv => sv.AppUserId);

            modelBuilder.Entity<VacancyProgrammingLanguage>()
                .HasKey(vpl => new { vpl.VacancyId, vpl.ProgrammingLanguageId });

            modelBuilder.Entity<VacancyProgrammingLanguage>()
                .HasOne(vpl => vpl.Vacancy)
                .WithMany(v => v.ProgrammingLanguages)
                .HasForeignKey(vpl => vpl.VacancyId);

            modelBuilder.Entity<VacancyProgrammingLanguage>()
                .HasOne(vpl => vpl.ProgrammingLanguage)
                .WithMany()
                .HasForeignKey(vpl => vpl.ProgrammingLanguageId);

            modelBuilder.Entity<VacancyFramework>()
                .HasKey(vf => new { vf.VacancyId, vf.FrameworkId });

            modelBuilder.Entity<VacancyFramework>()
                .HasOne(vf => vf.Vacancy)
                .WithMany(v => v.Frameworks)
                .HasForeignKey(vf => vf.VacancyId);

            modelBuilder.Entity<VacancyFramework>()
                .HasOne(vf => vf.Framework)
                .WithMany()
                .HasForeignKey(vf => vf.FrameworkId);
        }
    }
}
