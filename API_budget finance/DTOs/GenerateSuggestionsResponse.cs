namespace API_budget_finance.DTOs
{
    public class SuggestionItem
    {
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public string Price { get; set; } = "";
    }

    public class GenerateSuggestionsResponse
    {
        public List<SuggestionItem> Suggestions { get; set; } = [];
    }
}
