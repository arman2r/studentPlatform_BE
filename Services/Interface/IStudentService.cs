using EducationAPI.DTOs;
using EducationAPI.Models;

namespace EducationAPI.Services.Interface
{
    public interface IStudentService
    {
        Task<Student> RegisterStudentAsync(RegisterStudentDto dto);
        Task<StudentDetailDto> GetStudentDetailAsync(int studentId);
        Task<Dictionary<string, List<string>>> GetClassmatesByStudentIdAsync(int studentId);
        Task DeleteStudentAsync(int studentId);
        Task<List<StudentSummaryDto>> GetAllStudentsAsync();
        Task AssignSubjectsToStudentAsync(int studentId, List<SubjectAssignmentDto> subjects);

        Task<List<TeacherBySubjectDto>> GetTeachersBySubjectIdAsync(int subjectId);

    }
}
