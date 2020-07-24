using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.ActivateFinancialProject
{
    public class ActivateFinancialProjectCommandValidator : AbstractValidator<ActivateFinancialProjectCommand>
    {
        public ActivateFinancialProjectCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotNull()
                .NotEmpty();
        }
    }
}