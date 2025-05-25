using EducationAPI.DTOs;
using EducationAPI.Services;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterTeacherDto dto)
        {
            var teacher = await _teacherService.RegisterTeacherAsync(dto);
            return Ok(teacher);
        }

        [HttpPut("update-subjects")]
        public async Task<IActionResult> UpdateSubjects([FromBody] UpdateTeacherSubjectsDto dto)
        {
            await _teacherService.UpdateTeacherSubjectsAsync(dto.TeacherId, dto.SubjectIds);
            return Ok(new { message = "Materias actualizadas correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var detail = await _teacherService.GetTeacherDetailAsync(id);
            return Ok(detail);
        }

        // 3. Obtener compañeros de clase del estudiante
        [HttpGet("{id}/classmates")]
        public async Task<IActionResult> GetClassmates(int id)
        {
            var classmates = await _teacherService.GetClassmatesByTeacherIdAsync(id);
            return Ok(classmates);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return NoContent();
        }
    }
}
