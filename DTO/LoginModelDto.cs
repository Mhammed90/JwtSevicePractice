namespace JWTPractice.DTO
{
    public class LoginModelDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
