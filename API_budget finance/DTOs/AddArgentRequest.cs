using API_budget_finance.Models;
using System.ComponentModel.DataAnnotations;

namespace API_budget_finance.DTOs
{
    public class AddArgentRequest
    {
        public TypeArgent Type { get; set; }
        [MaxLength(70)]
        public string Description { get; set; }
        public double Montant { get; set; }
        public int CategoryId { get; set; }
    }
}
