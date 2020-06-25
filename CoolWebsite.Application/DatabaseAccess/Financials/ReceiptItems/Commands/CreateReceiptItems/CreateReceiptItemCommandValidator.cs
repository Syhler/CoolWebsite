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
                .NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ItemGroup)
                .NotNull();

            RuleFor(x => x.ReceiptId)
                .NotEmpty();

            RuleFor(x => x.UsersId)
                .NotNull()
                .NotEmpty();
        }
    }
}