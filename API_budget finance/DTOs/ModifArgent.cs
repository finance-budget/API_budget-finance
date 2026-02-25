using API_budget_finance.Models;

namespace API_budget_finance.DTOs
{
    public class ModifArgent
    {
        
        public TypeArgent Type { get; set; }
        public string Description { get; set; }
        public double Montant { get; set; }
        public int CategoryId { get; set; }
    }
}
