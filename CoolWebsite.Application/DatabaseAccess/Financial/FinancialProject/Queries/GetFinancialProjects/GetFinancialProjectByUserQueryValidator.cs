using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects
{
    public class GetFinancialProjectByUserQueryValidator : AbstractValidator<GetFinancialProjectsByUserQuery>
    {
        public GetFinancialProjectByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}