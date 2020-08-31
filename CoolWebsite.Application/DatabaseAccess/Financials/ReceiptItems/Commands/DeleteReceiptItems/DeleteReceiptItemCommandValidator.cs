using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems
{
    public class DeleteReceiptItemCommandValidator : AbstractValidator<DeleteReceiptItemCommand>
    {
        public DeleteReceiptItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.FinancialProjectId)
                .NotEmpty()
                .NotNull();
        }
    }
}