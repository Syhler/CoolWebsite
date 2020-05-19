using System.Data;
using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems
{
    public class CreateReceiptItemValidator : AbstractValidator<CreateReceiptItemCommand>
    {
        public CreateReceiptItemValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ItemGroup)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ReceiptId)
                .NotEmpty();
        }
    }
}