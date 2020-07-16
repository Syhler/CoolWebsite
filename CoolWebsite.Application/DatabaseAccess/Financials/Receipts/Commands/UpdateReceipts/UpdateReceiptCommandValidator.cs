using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts
{
    public class UpdateReceiptCommandValidator : AbstractValidator<UpdateReceiptCommand>
    {

        public UpdateReceiptCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
         
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100);

            RuleFor(x => x.Datevisited)
                .NotEmpty().WithMessage("BoughtAt is required");
            
            
        }
        
    }
}