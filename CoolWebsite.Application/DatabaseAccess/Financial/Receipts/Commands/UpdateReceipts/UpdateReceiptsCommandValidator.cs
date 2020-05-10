using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.UpdateReceipts
{
    public class UpdateReceiptsCommandValidator : AbstractValidator<UpdateReceiptsCommand>
    {

        public UpdateReceiptsCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Total)
                .GreaterThanOrEqualTo(0).WithMessage("Total has to be above 0");
            
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100);

            RuleFor(x => x.BoughtAt)
                .NotEmpty().WithMessage("BoughtAt is required");
            
        }
        
    }
}