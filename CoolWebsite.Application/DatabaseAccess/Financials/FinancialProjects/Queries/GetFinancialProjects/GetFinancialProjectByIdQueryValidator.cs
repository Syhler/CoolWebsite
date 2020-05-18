using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetFinancialProjectByIdQueryValidator : AbstractValidator<GetFinancialProjectByIdQuery>
    {
        public GetFinancialProjectByIdQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required");
        }
    }
}