using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.DeleteTransactions
{
    public class DeleteTransactionCommand : IRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTransactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Transactions
                .FirstOrDefault(x => x.Id == request.Id &&
                                     x.CreatedBy == _currentUserService.UserId);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Financial.Transaction), request.Id);
            }

            var oweRecord = _context.OweRecords.FirstOrDefault(x =>
                x.FinancialProjectId == entity.FinancialProjectId && 
                x.UserId == entity.FromUserId &&
                x.OwedUserId == entity.ToUserId);

            oweRecord.Amount += entity.Amount;

            _context.Transactions.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
    
            return Unit.Value;
        }

        
    }
}