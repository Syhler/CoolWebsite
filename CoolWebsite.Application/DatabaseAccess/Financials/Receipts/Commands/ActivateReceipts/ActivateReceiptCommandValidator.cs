using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.ActivateReceipts
{
    public class ActivateReceiptCommandValidator : AbstractValidator<ActivateReceiptCommand>
    {
        public ActivateReceiptCommandValidator()
        {
            RuleFor(x => x.ReceiptId)
                .NotNull()
                .NotEmpty();
        }
    }
}