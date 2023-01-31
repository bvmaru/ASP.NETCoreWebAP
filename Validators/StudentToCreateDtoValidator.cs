using ASP.NETCoreWebAP.DTOs;
using FluentValidation;

namespace ASP.NETCoreWebAP.Validators
{
    public class StudentToCreateDtoValidator : AbstractValidator<StudentToCreateDto>
    {
        public StudentToCreateDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }

}
