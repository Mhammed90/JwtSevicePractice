



namespace JWTPractice.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(64 ),MinLength(3)]
        public string FristName { get; set; }
        [Required, MaxLength(64), MinLength(3)]
        public string LastName { get; set; }

    }
}
