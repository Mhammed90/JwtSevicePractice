namespace JWTPractice.DTO
{
    public class RoleModelDto
    {
        [Required]
        public string UserId { get; set; }
       [Required, MaxLength(64), MinLength(3)]
        public string RoleName { get; set; }
    }

}
