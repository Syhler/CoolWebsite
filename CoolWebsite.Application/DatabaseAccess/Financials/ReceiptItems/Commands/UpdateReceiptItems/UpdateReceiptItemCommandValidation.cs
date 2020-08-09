using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems
{
    public class UpdateReceiptItemCommandValidation : AbstractValidator<UpdateReceiptItemCommand>
    {
        public UpdateReceiptItemCommandValidation()
        {
            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ItemGroup)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.UserDtos)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.FinancialProjectId)
                .NotNull()
                .NotEmpty();

        }
    }
}