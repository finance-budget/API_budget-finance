
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_budget_finance.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext <User>(options)
    {
        public DbSet<Argent> Argents { get; set; }
        public DbSet<Objectif> Objectifs { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
