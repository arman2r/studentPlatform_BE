namespace EducationAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<StudentSubject> StudentSubjects { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
