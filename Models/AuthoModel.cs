namespace JWTPractice.Models
{
    public class AuthoModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string Tokens { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
