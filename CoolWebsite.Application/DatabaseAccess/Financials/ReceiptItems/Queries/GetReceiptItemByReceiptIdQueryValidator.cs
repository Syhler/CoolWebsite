using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries
{
    public class GetReceiptItemByReceiptIdQueryValidator : AbstractValidator<GetReceiptItemByReceiptIdQuery>
    {
        public GetReceiptItemByReceiptIdQueryValidator()
        {
            RuleFor(x => x.ReceiptId)
                .NotEmpty()
                .NotNull();
        }
    }
}