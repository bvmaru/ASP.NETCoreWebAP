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

        [HttpGet("oldest student")]
        public IEnumerable<StudentAgeDto> GetStudentsOldestStudent()
        {
            var students = _dbContext.Students.ToList();
            int age = 0;
            List <Student> oldestStudents = new List<Student>();
            foreach (var student2 in students)
            {
                if (student2.Age > age)
                {
                    age = student2.Age;
                }
            }
            foreach (var student in students)
            {
                if(age == student.Age)
                {
                    oldestStudents.Add(student);
                }
            }

            return oldestStudents.Select(x => new StudentAgeDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName,
                Age = x.Age
            });
        }


        [HttpGet("youngest student")]
        public IEnumerable<StudentAgeDto> GetStudentsYoungestStudent()
        {
            var students = _dbContext.Students.ToList();
            int age = 0;
            if (students.Count != 0)
            {
                age = students[0].Age;
            }
            List<Student> youngestStudents = new List<Student>();
            foreach (var student2 in students)
            {
                if (student2.Age < age)
                {
                    age = student2.Age;
                }
            }
            foreach (var student in students)
            {
                if (age == student.Age)
                {
                    youngestStudents.Add(student);
                }
            }

            return youngestStudents.Select(x => new StudentAgeDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName,
                Age = x.Age
            });
        }

        [HttpGet("older than average student")]
        public IEnumerable<StudentAgeDto> GetStudentsOlderThanAveragetStudent()
        {
            var students = _dbContext.Students.ToList();
            var studentsCount = students.Count();
            var sumAge = students.Sum(x => x.Age);
            int age = sumAge / studentsCount;
            List<Student> olderStudents = new List<Student>();
            foreach (var student2 in students)
            {
                if (student2.Age > age)
                {
                    age = student2.Age;
                }
            }
            foreach (var student in students)
            {
                if (age == student.Age)
                {
                    olderStudents.Add(student);
                }
            }

            return olderStudents.Select(x => new StudentAgeDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName,
                Age = x.Age
            });
        }

        [HttpPut("edytuj status studenta {id}")]
        public ActionResult UpdateStudentStatus([FromRoute] int id, [FromBody] StudentIsActiveDto studentIsActiveDto)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            student.IsActive = studentIsActiveDto.IsActive;
            _dbContext.SaveChanges();
            return NoContent();
        }


    }
}
