namespace biVerifier.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? UName { get; set; }
        public string? UserName { get; set;}
        public string? Surname { get; set;}
        public string? UserPW { get; set;}
        public string? email { get; set;}

        public string? Security { get; set;}
        public string? Department { get; set;}
        public string? Status { get; set;}
        public string? Answer { get; set;}

        public string? Role { get; set; }
    }
}
