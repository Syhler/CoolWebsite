using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.DeleteTransactions
{
    public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
    {
        public DeleteTransactionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}