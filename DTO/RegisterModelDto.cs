namespace JWTPractice.DTO
{
    public class RegisterModelDto
    {
        [Required, MaxLength(64), MinLength(3)]
        public string FirstName { get; set; }
        [Required, MaxLength(64), MinLength(3)]
        public string LastName { get; set; }
        [Required, MaxLength(64), MinLength(3)]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
