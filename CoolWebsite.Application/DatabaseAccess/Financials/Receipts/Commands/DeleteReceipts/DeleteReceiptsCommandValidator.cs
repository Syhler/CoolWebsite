using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.DeleteReceipts
{
    public class DeleteReceiptsCommandValidator : AbstractValidator<DeleteReceiptsCommand>
    {
        public DeleteReceiptsCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}