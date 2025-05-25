using EducationAPI.DTOs;
using EducationAPI.Models;

namespace EducationAPI.Services.Interface
{
    public interface ITeacherService
    {
        Task<Teacher> RegisterTeacherAsync(RegisterTeacherDto dto);
        Task UpdateTeacherSubjectsAsync(int teacherId, List<int> subjectIds);
        Task<Dictionary<string, List<string>>> GetClassmatesByTeacherIdAsync(int teacherId);
        Task<List<TeacherDetailDto>> GetAllTeachersAsync();
        Task<TeacherDetailDto> GetTeacherDetailAsync(int id);
        Task DeleteTeacherAsync(int id);
    }
}
