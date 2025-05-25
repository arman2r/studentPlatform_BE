using EducationAPI.Data;
using EducationAPI.DTOs;
using EducationAPI.Models;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Teacher> _passwordHasher;

        public TeacherService(AppDbContext context, IPasswordHasher<Teacher> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Teacher> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            if (await _context.Teacher.AnyAsync(t => t.Email == dto.Email))
                throw new Exception("Email ya registrado.");

            var teacher = new Teacher
            {
                FullName = dto.FullName,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            teacher.PasswordHash = _passwordHasher.HashPassword(teacher, dto.PasswordHash);

            _context.Teacher.Add(teacher);
            await _context.SaveChangesAsync();

            return teacher;
        }

        public async Task UpdateTeacherSubjectsAsync(int teacherId, List<int> subjectIds)
        {
            var teacher = await _context.Teacher.FindAsync(teacherId);
            if (teacher == null)
                throw new Exception("Profesor no encontrado.");

            if (subjectIds.Count > 2)
                throw new Exception("Un profesor no puede tener más de 2 materias.");

            var subjects = await _context.Subject
                .Where(s => subjectIds.Contains(s.Id))
                .ToListAsync();

            if (subjects.Count != subjectIds.Count)
                throw new Exception("Una o más materias no existen.");

            // Eliminar relaciones anteriores
            var existingRelations = await _context.TeacherSubject
                .Where(ts => ts.TeacherId == teacherId)
                .ToListAsync();

            _context.TeacherSubject.RemoveRange(existingRelations);

            // Agregar nuevas relaciones
            var newRelations = subjectIds.Select(subjectId => new TeacherSubject
            {
                TeacherId = teacherId,
                SubjectId = subjectId
            });

            await _context.TeacherSubject.AddRangeAsync(newRelations);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TeacherDetailDto>> GetAllTeachersAsync()
        {
            return await _context.Teacher
                .Select(t => new TeacherDetailDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    Email = t.Email,
                    Subjects = _context.TeacherSubject
                        .Where(ss => ss.TeacherId == t.Id)
                        .Select(ss => new SubjectDto
                        {
                            Id = ss.Subject.Id,
                            Name = ss.Subject.Name,
                            Credits = ss.Subject.Credits
                        }).Distinct().ToList()
                }).ToListAsync();
        }

        public async Task<Dictionary<string, List<string>>> GetClassmatesByTeacherIdAsync(int teacherId)
        {
            var classmates = await _context.StudentSubject
                .Where(ss => ss.TeacherId == teacherId)
                .SelectMany(ss => _context.StudentSubject
                    .Where(other => other.SubjectId == ss.SubjectId && other.TeacherId == teacherId)
                    .Select(other => new
                    {
                        SubjectName = ss.Subject.Name,
                        ClassmateName = other.Student.FullName
                    }))
                .ToListAsync();

            return classmates
                .GroupBy(c => c.SubjectName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c => c.ClassmateName).Distinct().ToList()
                );
        }

        public async Task<TeacherDetailDto> GetTeacherDetailAsync(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null) throw new Exception("No encontrado");

            var subjectList = await _context.TeacherSubject
                .Where(ts => ts.TeacherId == id)
                .Select(ts => new SubjectDto
                {
                    Id = ts.Subject.Id,
                    Name = ts.Subject.Name
                })
                .ToListAsync();

            return new TeacherDetailDto
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                Email = teacher.Email,
                Subjects = subjectList
            };
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null) throw new Exception("No encontrado");
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
