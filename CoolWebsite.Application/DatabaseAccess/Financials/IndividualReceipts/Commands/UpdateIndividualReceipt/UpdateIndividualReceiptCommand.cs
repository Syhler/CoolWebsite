using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.UpdateIndividualReceipt
{
    public class UpdateIndividualReceiptCommand : IRequest
    {
        public string Id { get; set; }
        
        public double Total { get; set; }
        
        public string UserId { get; set; }

        public string ReceiptId { get; set; }
    }

    public class UpdateIndividualReceiptCommandHandler : IRequestHandler<UpdateIndividualReceiptCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateIndividualReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }


        public async Task<Unit> Handle(UpdateIndividualReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.IndividualReceipts.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualReceiptObsolete), request.Id);
            }

            entity.Total = request.Total;

            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                entity.UserId = request.UserId;
            }
            
            if (!string.IsNullOrWhiteSpace(request.ReceiptId))
            {
                entity.ReceiptId = request.ReceiptId;
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;

        }
    }
}