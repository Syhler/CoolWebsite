using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.UpdateIndividualReceipt
{
    public class UpdateIndividualReceiptCommandValidator : AbstractValidator<UpdateIndividualReceiptCommand>
    {
        public UpdateIndividualReceiptCommandValidator()
        {
            RuleFor(x => x.Total)
                .GreaterThanOrEqualTo(0);

        }
    }
}