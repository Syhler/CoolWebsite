using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject
{
    public class DeleteFinancialProjectCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteFinancialProjectCommandHandler : IRequestHandler<DeleteFinancialProjectCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<Unit> Handle(DeleteFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.FinancialProjects
                .Include(x => x.Receipts)
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include("Receipts.Receptors")
                .FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Financial.FinancialProject), request.Id);
            }
            
            var receipts = entity.Receipts;
            /*
            foreach (var receipt in receipts)
            {
                foreach (var individualReceipt in receipt.Receptors)
                {
                    _context.IndividualReceipts.Remove(individualReceipt);
                }

                _context.Receipts.Remove(receipt);
            }
            */
            var users = entity.FinancialProjectApplicationUsers;
            
            foreach (var financialProjectApplicationUser in users)
            {
                _context.FinancialProjectApplicationUsers.Remove(financialProjectApplicationUser);
            }


            _context.FinancialProjects.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}