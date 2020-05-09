using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.DeleteReceipts
{
    public class DeleteReceiptsCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteReceiptsCommandHandler : IRequestHandler<DeleteReceiptsCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteReceiptsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<Unit> Handle(DeleteReceiptsCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Receipts.Find(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            
            _context.Receipts.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}