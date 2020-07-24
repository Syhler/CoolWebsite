using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.ActivateFinancialProject
{
    public class ActivateFinancialProjectCommand : IRequest
    {
        public string ProjectId { get; set; } = null!;
    }

    public class ActivateFinancialProjectCommandHandler : IRequestHandler<ActivateFinancialProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;


        public ActivateFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ActivateFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.FinancialProjects
                .FirstOrDefault(x => x.Id == request.ProjectId && x.CreatedBy == _currentUserService.UserId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(FinancialProject), request.ProjectId);
            }

            entity.Deleted = null;
            entity.DeletedByUserId = null;

            await _context.SaveChangesAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}