using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.UpdateFinancialProject
{
    public class UpdateFinancialProjectValidator : AbstractValidator<UpdateFinancialProjectCommand>
    {
        public UpdateFinancialProjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(2)
                .MaximumLength(100);
        }
    }
}