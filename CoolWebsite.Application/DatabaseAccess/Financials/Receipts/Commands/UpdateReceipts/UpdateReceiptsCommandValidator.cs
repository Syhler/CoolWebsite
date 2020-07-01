using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts;
using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.UpdateReceipts
{
    public class UpdateReceiptsCommandValidator : AbstractValidator<UpdateReceiptsCommand>
    {

        public UpdateReceiptsCommandValidator()
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