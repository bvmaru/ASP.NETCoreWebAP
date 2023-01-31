using ASP.NETCoreWebAP.Domain;
using ASP.NETCoreWebAP.DTOs;
using ASP.NETCoreWebAP.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreWebAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private StudentsDBContext _dbContext;
        public StudentsController(StudentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<StudentNameDto> GetStudentsNames()
        {
            var students = _dbContext.Students.ToList();
            return students.Select(x => new StudentNameDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName
            });
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDto> GetStudent([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            var studentDto = new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                IsActive = student.IsActive
            };
            return Ok(studentDto);
        }

        [HttpGet("average-age")]
        public int GetStudentsAverageAge()
        {
            var students = _dbContext.Students.ToList();
            var studentsCount = students.Count();
            var sumAge = students.Sum(x => x.Age);
            return sumAge / studentsCount;
        }

        [HttpPost]
        public ActionResult<StudentDto> AddStudent([FromBody] StudentToCreateDto studentToCreateDto)
        {
            var student = new Student
            {
                Name = studentToCreateDto.Name,
                LastName = studentToCreateDto.LastName,
                Email = studentToCreateDto.Email,
                Age = studentToCreateDto.Age,
                IsActive = studentToCreateDto.IsActive
            };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            var studentDto = new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                IsActive = student.IsActive
            };
            return Created(string.Empty, studentDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateStudent([FromRoute] int id, [FromBody] StudentToUpdateDto studentToUpdateDto)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            student.Name = studentToUpdateDto.Name;
            student.LastName = studentToUpdateDto.LastName;
            student.Email = studentToUpdateDto.Email;
            student.Age = studentToUpdateDto.Age;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Deletetudent([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return NoContent();
        }


    }
}
