using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt
{
    public class CreateIndividualReceiptCommandValidator : AbstractValidator<CreateIndividualReceiptCommand>
    {
        public CreateIndividualReceiptCommandValidator()
        {
            RuleFor(x => x.Total)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ReceiptId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}