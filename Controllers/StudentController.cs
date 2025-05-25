using EducationAPI.DTOs;
using EducationAPI.Services;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterStudentDto studentDto)
        {
            try
            {                
                var student = await _studentService.RegisterStudentAsync(studentDto);
                return Ok(new { student.Id, student.FullName, student.Email });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut("{studentId}/assign-subjects")]
        public async Task<IActionResult> AssignSubjects(int studentId, [FromBody] List<SubjectAssignmentDto> subjects)
        {
            try
            {
                await _studentService.AssignSubjectsToStudentAsync(studentId, subjects);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. Obtener detalle del estudiante por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var student = await _studentService.GetStudentDetailAsync(id);
            return Ok(student);
        }

        // 3. Obtener compañeros de clase del estudiante
        [HttpGet("{id}/classmates")]
        public async Task<IActionResult> GetClassmates(int id)
        {
            var classmates = await _studentService.GetClassmatesByStudentIdAsync(id);
            return Ok(classmates);
        }

        [HttpGet("{subjectId}/teachers")]
        public async Task<IActionResult> GetTeachersBySubjectId(int subjectId)
        {
            var result = await _studentService.GetTeachersBySubjectIdAsync(subjectId);
            return Ok(result);
        }

        // 4. Eliminar estudiante
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }

        // 5. (opcional) Listar todos los estudiantes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync(); // si implementas este método
            return Ok(students);
        }
    }
}
