using FluentValidation;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.UpdateFinancialProject
{
    public class UpdateFinancialProjectValidator : AbstractValidator<UpdateFinancialProjectCommand>
    {
        public UpdateFinancialProjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100);

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Users)
                .NotEmpty().WithMessage("Users are required");

        }
    }
}