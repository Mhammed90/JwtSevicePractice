

/// I used this to quick access and get rid of magic strings

namespace JWTPractice.Helper
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issure { get; set; }
        public string Audience { get; set; }
        public double ExpiresInDayes { get; set; }
    }
}
