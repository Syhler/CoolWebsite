using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts
{
    public class CreateReceiptsCommandValidator : AbstractValidator<CreateReceiptsCommand>
    {
        public CreateReceiptsCommandValidator()
        {
            RuleFor(x => x.Total)
                .GreaterThanOrEqualTo(0).WithMessage("Total has to be above 0");

            RuleFor(x => x.FinancialProjectId)
                .NotEmpty().WithMessage("FinancialProjectID is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100);

            RuleFor(x => x.BoughtAt)
                .NotEmpty().WithMessage("BoughtAt is required");
        }
    }
}