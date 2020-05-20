using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems
{
    public class DeleteReceiptItemCommandValidator : AbstractValidator<DeleteReceiptItemCommand>
    {
        public DeleteReceiptItemCommandValidator()
        {
            RuleFor(x => x.ReceiptId)
                .NotEmpty();
        }
    }
}