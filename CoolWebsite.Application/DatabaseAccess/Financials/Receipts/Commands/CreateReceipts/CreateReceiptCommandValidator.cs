using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts
{
    public class CreateReceiptCommandValidator : AbstractValidator<CreateReceiptCommand>
    {
        public CreateReceiptCommandValidator()
        {

            RuleFor(x => x.FinancialProjectId)
                .NotEmpty().WithMessage("FinancialProjectID is required");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100);

            RuleFor(x => x.DateVisited)
                .NotEmpty().WithMessage("BoughtAt is required");
            
            
        }
    }
}