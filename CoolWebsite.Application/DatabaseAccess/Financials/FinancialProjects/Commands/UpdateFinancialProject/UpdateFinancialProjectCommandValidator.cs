using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.UpdateFinancialProject
{
    public class UpdateFinancialProjectCommandValidator : AbstractValidator<UpdateFinancialProjectCommand>
    {
        public UpdateFinancialProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100)
                .NotNull();

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotNull();

            RuleFor(x => x.Users)
                .NotEmpty().WithMessage("Users are required")
                .NotNull().WithMessage("Users are required");

        }
    }
}