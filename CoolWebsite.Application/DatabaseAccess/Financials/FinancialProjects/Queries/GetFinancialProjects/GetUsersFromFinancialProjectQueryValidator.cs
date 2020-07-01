using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetUsersFromFinancialProjectQueryValidator : AbstractValidator<GetUsersFromFinancialProjectQuery>
    {
        public GetUsersFromFinancialProjectQueryValidator()
        {
            RuleFor(x => x.FinancialProjectId)
                .NotEmpty()
                .NotNull();
        }
    }
}