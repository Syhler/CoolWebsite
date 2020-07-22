using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts
{
    public class UpdateReceiptCommandValidator : AbstractValidator<UpdateReceiptCommand>
    {

        public UpdateReceiptCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotNull();

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100)
                .NotNull();

            RuleFor(x => x.DateVisited)
                .NotEmpty().WithMessage("BoughtAt is required");

            RuleFor(x => x.ItemDtos)
                .NotNull()
                .NotEmpty();

        }
        
    }
}