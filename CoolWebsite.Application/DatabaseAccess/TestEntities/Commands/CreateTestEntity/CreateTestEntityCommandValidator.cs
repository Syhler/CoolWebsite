using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity
{
    public class CreateTestEntityCommandValidator : AbstractValidator<CreateTestEntityCommand>
    {
        public CreateTestEntityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(10).WithMessage("Max 10 length");
        }
    }
}