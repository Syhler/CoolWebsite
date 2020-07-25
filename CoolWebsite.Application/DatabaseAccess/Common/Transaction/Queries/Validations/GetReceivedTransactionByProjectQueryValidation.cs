using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Validations
{
    public class GetReceivedTransactionByProjectQueryValidation : AbstractValidator<GetReceivedTransactionByProjectQuery>
    {
        public GetReceivedTransactionByProjectQueryValidation()
        {
            RuleFor(x => x.FinancialProjectId)
                .NotNull()
                .NotEmpty();
        }
    }
}