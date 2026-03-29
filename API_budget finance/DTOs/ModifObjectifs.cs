namespace API_budget_finance.DTOs
{
    public class ModifObjectifs
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public bool Atteint { get; set; }
    }
}
