using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts
{
    public class DeleteReceiptCommandValidator : AbstractValidator<DeleteReceiptCommand>
    {
        public DeleteReceiptCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}