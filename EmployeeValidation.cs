using FluentValidation;

namespace GillCleerenBlazorApi;

public class EmployeeValidation : AbstractValidator<Employee>
{
    public EmployeeValidation()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
    }
}
