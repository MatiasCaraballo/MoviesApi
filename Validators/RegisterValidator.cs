namespace MoviesApp.Validators;

using FluentValidation;
public class UserDtoValidator : AbstractValidator<RegisterDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The email is required")
            .EmailAddress().WithMessage("Must be an valid mail");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The password is required")
            .MinimumLength(6).WithMessage("Password must been at least 6 cracters long");
    }
}