using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Queries
{
    public class GetReceiptByIdQueryValidator : AbstractValidator<GetReceiptByIdQueryVm>
    {
        public GetReceiptByIdQueryValidator()
        {
            RuleFor(x => x.ReceiptId)
                .NotEmpty();
        }
    }
}