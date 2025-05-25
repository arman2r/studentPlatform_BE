using EducationAPI.Data;
using EducationAPI.DTOs;
using EducationAPI.Models;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost]
        public async Task<ActionResult<SubjectDto>> Create(CreateSubjectDto dto)
        {
            var subject = await _subjectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
        }

        [HttpGet]
        public async Task<ActionResult<List<SubjectDto>>> GetAll()
        {
            return await _subjectService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null) return NotFound();
            return subject;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _subjectService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
