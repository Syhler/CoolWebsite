using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Validations
{
    public class GetPayedTransactionByProjectQueryValidation : AbstractValidator<GetPayedTransactionByProjectQuery>
    {
        public GetPayedTransactionByProjectQueryValidation()
        {
            RuleFor(x => x.FinancialProjectId)
                .NotNull()
                .NotEmpty();
        }
    }
}