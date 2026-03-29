using System.ComponentModel.DataAnnotations;

namespace API_budget_finance.Models
{
    public class Objectif
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public bool Atteint { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        
    }
}
