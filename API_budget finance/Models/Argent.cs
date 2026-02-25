using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_budget_finance.Models
{
    public enum TypeArgent
    {
        Depense,
        Revenu
    }

    
    public class Argent
    {
        public int Id { get ; set; }
        public DateTime DateOperation { get; set; }

        public TypeArgent Type { get; set; }
        [MaxLength(70)]
        public string Description { get; set; }
        public double Montant { get; set; }
        

        public int CategoryId { get; set; } 
        public Category Category { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
