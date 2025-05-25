namespace EducationAPI.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public List<TeacherDetailDto> Teacher { get; set; } = new();
    }
}
