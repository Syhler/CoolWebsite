using System;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts
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
            var entity = await _context.Receipts.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            
            entity.Deleted = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}