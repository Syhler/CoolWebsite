using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems
{
    public class DeleteReceiptItemCommand : IRequest
    {
        public string ReceiptId { get; set; }
    }
    
    public class DeleteReceiptItemCommandHandler : IRequestHandler<DeleteReceiptItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteReceiptItemCommandHandler(IApplicationDbContext context, ICurrentUserService userService)
        {
            _context = context;
            _context.UserId = userService.UserID;
        }


        public async Task<Unit> Handle(DeleteReceiptItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ReceiptItems.FindAsync(request.ReceiptId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ReceiptItem), request.ReceiptId);
            }

            _context.ReceiptItems.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}