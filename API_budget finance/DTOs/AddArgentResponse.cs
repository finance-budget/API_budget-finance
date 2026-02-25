using API_budget_finance.Models;
using System.ComponentModel.DataAnnotations;

namespace API_budget_finance.DTOs
{
    public class AddArgentResponse
    {
        public int Id { get; set; }
        public TypeArgent Type { get; set; }
        [MaxLength(70)]
        public string Description { get; set; }
        public double Montant { get; set; }
        public int CategoryId { get; set; }
        public DateTime DateOperation { get; set; }
    }
}
