namespace EducationAPI.DTOs
{
    public class UpdateTeacherSubjectsDto
    {
        public int TeacherId { get; set; }
        public List<int> SubjectIds { get; set; } = new();
    }
}
