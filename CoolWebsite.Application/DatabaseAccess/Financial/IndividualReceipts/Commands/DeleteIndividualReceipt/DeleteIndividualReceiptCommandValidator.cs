using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.DeleteIndividualReceipt
{
    public class DeleteIndividualReceiptCommandValidator : AbstractValidator<DeleteIndividualReceiptCommand>
    {
        public DeleteIndividualReceiptCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}