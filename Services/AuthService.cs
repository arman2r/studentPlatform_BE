using EducationAPI.Data;
using EducationAPI.DTOs;
using EducationAPI.Models;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducationAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Teacher> _teacherHasher;
        private readonly IPasswordHasher<Student> _studentHasher;
        private readonly IConfiguration _config;

        public AuthService(
            AppDbContext context,
            IPasswordHasher<Teacher> teacherHasher,
            IPasswordHasher<Student> studentHasher,
            IConfiguration config)
        {
            _context = context;
            _teacherHasher = teacherHasher;
            _studentHasher = studentHasher;
            _config = config;
        }

        public async Task<string> LoginAsTeacher(LoginDto dto)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.Email == dto.Email);
            if (teacher == null)
                throw new Exception("Profesor no encontrado");

            Console.WriteLine($"Contraseña ingresada: '{dto.Password}'");
            var result = _teacherHasher.VerifyHashedPassword(teacher, teacher.PasswordHash, dto.Password);
            Console.WriteLine($"Resultado verificación: {result}");
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Contraseña incorrecta");

            return GenerateToken(teacher.Id, teacher.FullName, teacher.Email, "Teacher");
        }

        public async Task<string> LoginAsStudent(LoginDto dto)
        {
            var student = await _context.Student.FirstOrDefaultAsync(s => s.Email == dto.Email);
            if (student == null)
                throw new Exception("Estudiante no encontrado");

            var result = _studentHasher.VerifyHashedPassword(student, student.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Contraseña incorrecta");

            return GenerateToken(student.Id, student.FullName, student.Email, "Student");
        }

        private string GenerateToken(int id, string name, string email, string role)
        {
            var claims = new[]
            {
               new Claim("Id", id.ToString()), // Use a custom claim type "Id" and convert int to string  
               new Claim(ClaimTypes.Name, name),
               new Claim(ClaimTypes.Email, email),
               new Claim(ClaimTypes.Role, role)
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
