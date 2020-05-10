using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject
{
    public class CreateFinancialProjectCommandValidator : AbstractValidator<CreateFinancialProjectCommand>
    {
        public CreateFinancialProjectCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100);

            RuleFor(x => x.Users)
                .NotEmpty().WithMessage("User is required");
        }
    }
}