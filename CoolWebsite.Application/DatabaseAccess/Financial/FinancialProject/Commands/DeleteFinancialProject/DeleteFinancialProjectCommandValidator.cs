using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.DeleteFinancialProject
{
    public class DeleteFinancialProjectCommandValidator : AbstractValidator<DeleteFinancialProjectCommand>
    {
        public DeleteFinancialProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}