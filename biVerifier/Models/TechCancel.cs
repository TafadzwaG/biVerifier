namespace biVerifier.Models
{
    public class TechCancel
    {
        public string? Client { get; set; }
        public string? SiteID { get; set; }
        public string? Date { get; set; }

        public string? TechResponsible { get; set; }

        public int? Total_Channels { get; set; }
        public string? Platform { get; set; }
        public string? Cancel_GSM { get; set; }

        public string? Cancel_DNS { get; set; }

        public string? Cancel_LPR_Licenses { get; set; }

        public string? Cancel_Video_Analytics_Licenses { get; set; }

        public string? Cancel_Internet_Connectivity { get; set; }

        public string? Cancel_Billing { get; set; }
    }
}
