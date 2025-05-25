namespace EducationAPI.DTOs
{
    public class TeacherBySubjectDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
