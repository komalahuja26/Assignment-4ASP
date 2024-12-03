using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using assignment2herhealth.Models;

namespace assignment2herhealth.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<assignment2herhealth.Models.PeriodEntry> PeriodEntry { get; set; } = default!;
        public DbSet<assignment2herhealth.Models.HealthSuggestion> HealthSuggestion { get; set; } = default!;
    }
}
