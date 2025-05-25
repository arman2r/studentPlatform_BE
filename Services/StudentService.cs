using EducationAPI.Data;
using EducationAPI.DTOs;
using EducationAPI.Models;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Student> _passwordHasher;

        public StudentService(AppDbContext context, IPasswordHasher<Student> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Student> RegisterStudentAsync(RegisterStudentDto dto)
        {
            if (await _context.Student.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("El correo ya está registrado.");

            var student = new Student
            {
                FullName = dto.FullName,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            student.PasswordHash = _passwordHasher.HashPassword(student, dto.PasswordHash);

            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task AssignSubjectsToStudentAsync(int studentId, List<SubjectAssignmentDto> subjects)
        {
            if (subjects.Count > 3)
                throw new Exception("Un estudiante no puede tener más de 3 materias.");

            var student = await _context.Student.FindAsync(studentId);
            if (student == null)
                throw new Exception("Estudiante no encontrado.");

            var subjectIds = subjects.Select(s => s.SubjectId).ToList();
            var dbSubjects = await _context.Subject
                .Where(s => subjectIds.Contains(s.Id))
                .ToListAsync();

            if (dbSubjects.Count != subjects.Count)
                throw new Exception("Una o más materias no existen.");

            // Validación: que no haya más de una materia con el mismo profesor en esta asignación
            var duplicates = subjects.GroupBy(s => s.TeacherId).Where(g => g.Count() > 1);
            if (duplicates.Any())
                throw new Exception("No se puede asignar más de una materia con el mismo profesor.");

            // Validar que un estudiante no tenga más de 3 materias asignadas (globalmente)
            /*foreach (var s in subjects)
            {
                var count = await _context.StudentSubject.CountAsync(ss => ss.TeacherId == s.TeacherId);
                if (count >= 2)
                    throw new Exception($"Ya tenías asignada previamente esa materia.");
            }*/

            // Eliminar asignaciones anteriores si existen
            var existingSubjects = await _context.StudentSubject
                .Where(ss => ss.StudentId == studentId)
                .ToListAsync();

            _context.StudentSubject.RemoveRange(existingSubjects);

            // Agregar nuevas materias
            foreach (var s in subjects)
            {
                _context.StudentSubject.Add(new StudentSubject
                {
                    StudentId = studentId,
                    SubjectId = s.SubjectId,
                    TeacherId = s.TeacherId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentSummaryDto>> GetAllStudentsAsync()
        {
            return await _context.Student
                .Select(s => new StudentSummaryDto
                {
                    Id = s.Id,
                    FullName = s.FullName
                })
                .ToListAsync();
        }

        // Obtener compañeros
        public async Task<Dictionary<string, List<string>>> GetClassmatesByStudentIdAsync(int studentId)
        {
            var classmates = await _context.StudentSubject
                .Where(ss => ss.StudentId == studentId)
                .SelectMany(ss => _context.StudentSubject
                    .Where(other => other.SubjectId == ss.SubjectId && other.StudentId != studentId)
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

        public async Task<StudentDetailDto> GetStudentDetailAsync(int studentId)
        {
            var student = await _context.Student
                .Where(s => s.Id == studentId)
                .Select(s => new StudentDetailDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Email = s.Email,
                    CreatedAt = s.CreatedAt,
                    Subjects = s.StudentSubjects.Select(ss => new SubjectInfoDto
                    {
                        SubjectId = ss.Subject.Id,
                        SubjectName = ss.Subject.Name,
                        TeacherId = ss.Teacher.Id,
                        TeacherName = ss.Teacher.FullName
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (student == null)
                throw new Exception("Estudiante no encontrado.");

            return student;
        }

        public async Task<List<TeacherBySubjectDto>> GetTeachersBySubjectIdAsync(int subjectId)
        {
            var teachers = await _context.TeacherSubject
                .Where(ts => ts.SubjectId == subjectId)
                .Include(ts => ts.Teacher)
                .Include(ts => ts.Subject)
                .Select(ts => new TeacherBySubjectDto
                {
                    TeacherId = ts.TeacherId,
                    TeacherName = ts.Teacher.FullName,
                    SubjectId = ts.SubjectId,
                    SubjectName = ts.Subject.Name
                })
                .ToListAsync();

            return teachers;
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await _context.Student
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                throw new Exception("Estudiante no encontrado.");

            _context.StudentSubject.RemoveRange(student.StudentSubjects);
            _context.Student.Remove(student);

            await _context.SaveChangesAsync();
        }

    }
}
