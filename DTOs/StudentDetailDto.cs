namespace EducationAPI.DTOs
{
    public class StudentDetailDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SubjectInfoDto> Subjects { get; set; }
    }
    
}
