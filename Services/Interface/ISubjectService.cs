using EducationAPI.DTOs;

namespace EducationAPI.Services.Interface
{
    public interface ISubjectService
    {
        Task<SubjectDto> CreateAsync(CreateSubjectDto dto);
        Task<List<SubjectDto>> GetAllAsync();
        Task<SubjectDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
