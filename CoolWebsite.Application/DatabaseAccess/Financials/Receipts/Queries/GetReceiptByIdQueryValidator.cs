using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries
{
    public class GetReceiptByIdQueryValidator : AbstractValidator<GetReceiptByIdQuery>
    {
        public GetReceiptByIdQueryValidator()
        {
            RuleFor(x => x.ReceiptId)
                .NotEmpty()
                .NotNull();
        }
    }
}