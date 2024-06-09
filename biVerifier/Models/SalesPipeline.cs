namespace biVerifier.Models
{
    public class SalesPipeline
    {
        public int QuoteNum { get; set; }
        public string? Client { get; set; }
        public string? Lead_Source { get; set; }

        public string? Contact_Person { get; set; }

        public string? Email { get; set; }
        public string? City { get; set;}

        public string? Service { get; set;}
        public string? Lead_Month { get; set; }
        public string? Consultant { get; set; }

        public string? OnceOffCost { get; set; }

        public string? RecurringCost { get; set; }

        public string? Probability { get; set; }

        public string? Market { get; set; }


    }
}
