using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentData;
using StudentData.DTOs;
using StudentData.Models;
using System.Text.RegularExpressions;

namespace StudentManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _context;
        private readonly IMapper _mapper;

        public StudentController(StudentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAll()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(_mapper.Map<List<StudentDTO>>(students));
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = "Студент не найден" });

            return Ok(_mapper.Map<StudentDTO>(student));
        }

       
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> Create(CreateStudentDTO dto)
        {
            if (!IsValidEmail(dto.Email))
                return BadRequest(new { message = "Некорректный формат email" });

            var student = _mapper.Map<Student>(dto);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<StudentDTO>(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, result);
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateStudentDTO dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = "Студент не найден" });

            if (!IsValidEmail(dto.Email))
                return BadRequest(new { message = "Некорректный формат email" });

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.BirthDate = dto.BirthDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = "Студент не найден" });

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

     
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }
}
