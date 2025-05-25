namespace EducationAPI.Models
{
    public class StudentSubject
    {
        public int Id { get; set; }

        // Relación con Student
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // Relación con Subject
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        // Relación con Teacher
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
