using System;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt
{
    public class CreateIndividualReceiptCommand : IRequest<string>
    {
        public double Total { get; set; }
        
        public string UserId { get; set; }

        public string ReceiptId { get; set; }
        
    }

    public class CreateIndividualReceiptCommandHandler : IRequestHandler<CreateIndividualReceiptCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateIndividualReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }
        
        
        public async Task<string> Handle(CreateIndividualReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = new IndividualReceiptObsolete
            {
                ReceiptId = request.ReceiptId,
                UserId = request.UserId,
                Total = request.Total,
                Id = Guid.NewGuid().ToString()
            };

            await _context.IndividualReceipts.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}