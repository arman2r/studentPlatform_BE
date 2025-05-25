using EducationAPI.DTOs;

namespace EducationAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsTeacher(LoginDto dto);
        Task<string> LoginAsStudent(LoginDto dto);
    }
}
