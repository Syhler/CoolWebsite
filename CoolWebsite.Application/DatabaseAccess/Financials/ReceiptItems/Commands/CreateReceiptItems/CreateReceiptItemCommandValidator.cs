using System.Data;
using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems
{
    public class CreateReceiptItemCommandValidator : AbstractValidator<CreateReceiptItemCommand>
    {
        public CreateReceiptItemCommandValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ItemGroup)
                .NotNull()
                .GreaterThanOrEqualTo(0);
            

            RuleFor(x => x.ReceiptId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.UserIds)
                .NotNull()
                .NotEmpty();
        }
    }
}