namespace EducationAPI.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Credits { get; set; }

        public List<StudentSubject> StudentSubjects { get; set; } = new();
        public List<TeacherSubject> TeacherSubjects { get; set; } = new();


    }
}
