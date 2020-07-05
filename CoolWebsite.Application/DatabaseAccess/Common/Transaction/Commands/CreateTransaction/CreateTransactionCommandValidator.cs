using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction
{
    public class CreateTransactionCommandValidation : AbstractValidator<CreateTransactionCommand>
    {

        public CreateTransactionCommandValidation()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.TransactionType)
                .NotNull();

            RuleFor(x => x.FinancialProjectId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.ToUserId)
                .NotEmpty()
                .NotNull();
        }
        
    }
}