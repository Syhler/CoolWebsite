using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.DeleteIndividualReceipt
{
    public class DeleteIndividualReceiptCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteIndividualReceiptCommandHandler : IRequestHandler<DeleteIndividualReceiptCommand>
    {
        
        private readonly IApplicationDbContext _context;

        public DeleteIndividualReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<Unit> Handle(DeleteIndividualReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.IndividualReceipts.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualReceipt), request.Id);
            }

            _context.IndividualReceipts.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
    
}