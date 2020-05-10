using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.CreateReceipts
{
    public class CreateReceiptsCommand : IRequest<string>
    {
        public double Total { get; set; }
        public string FinancialProjectId { get; set; }

        public List<IndividualReceipt> Receiptors { get; set; }
    }

    public class CreateReceiptsCommandHandler : IRequestHandler<CreateReceiptsCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateReceiptsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }


        public async Task<string> Handle(CreateReceiptsCommand request, CancellationToken cancellationToken)
        {
            
            var entity = new Receipt
            {
                Total = request.Total,
                FinancialProjectId = request.FinancialProjectId,
                Id = Guid.NewGuid().ToString(),
                Receptors = request.Receiptors ?? new List<IndividualReceipt>()
            };

            await _context.Receipts.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}