using Microsoft.AspNetCore.Identity;

namespace API_budget_finance.Models
{
    public class User: IdentityUser
    {
        public List<Argent> Argents { get; set; } = [];
        public List<Objectif> Objectifs { get; set; } = [];
    }
}
