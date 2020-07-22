using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetFinancialProjectByUserIdQueryValidator : AbstractValidator<GetFinancialProjectsByUserIdQuery>
    {
        public GetFinancialProjectByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .NotNull();
        }
    }
}