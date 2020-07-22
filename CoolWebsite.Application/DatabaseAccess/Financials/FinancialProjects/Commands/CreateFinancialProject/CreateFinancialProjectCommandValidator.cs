using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject
{
    public class CreateFinancialProjectCommandValidator : AbstractValidator<CreateFinancialProjectCommand>
    {
        public CreateFinancialProjectCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100)
                .NotNull();

            RuleFor(x => x.Users)
                .NotEmpty().WithMessage("User is required")
                .NotNull().WithMessage("User is required");
        }
    }
}