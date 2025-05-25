using System.ComponentModel.DataAnnotations;

namespace EducationAPI.DTOs
{
    public class RegisterStudentDto
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
         
    }
}
