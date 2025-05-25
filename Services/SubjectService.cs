using EducationAPI.Data;
using EducationAPI.DTOs;
using EducationAPI.Models;
using EducationAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Services
{
    public class SubjectService : ISubjectService
    {

        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SubjectDto> CreateAsync(CreateSubjectDto dto)
        {
            var subject = new Subject
            {
                Name = dto.Name,
                Credits = dto.Credits
            };

            _context.Subject.Add(subject);
            await _context.SaveChangesAsync();

            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Credits = subject.Credits
            };
        }

        public async Task<List<SubjectDto>> GetAllAsync()
        {
            return await _context.Subject
                .Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Credits = s.Credits,
                    Teacher = s.TeacherSubjects
                        .Select(ts => new TeacherDetailDto
                        {
                            Id = ts.Teacher.Id,
                            FullName = ts.Teacher.FullName,
                            Email = ts.Teacher.Email,
                            Subjects = null // O puedes mapear las materias si lo necesitas
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            var subject = await _context.Subject.FindAsync(id);
            if (subject == null) return null;

            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Credits = subject.Credits
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _context.Subject.FindAsync(id);
            if (subject == null) return false;

            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
