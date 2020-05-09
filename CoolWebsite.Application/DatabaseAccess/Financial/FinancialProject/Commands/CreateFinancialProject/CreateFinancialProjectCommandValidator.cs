using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject
{
    public class CreateFinancialProjectCommandValidator : AbstractValidator<CreateFinancialProjectCommand>
    {
        public CreateFinancialProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(2)
                .MaximumLength(100);
        }
    }
}