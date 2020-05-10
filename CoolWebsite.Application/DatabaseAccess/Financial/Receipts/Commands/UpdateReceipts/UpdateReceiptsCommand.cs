using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.UpdateReceipts
{
    public class UpdateReceiptsCommand : IRequest
    {
        public string Id { get; set; }
        public double Total { get; set; }
        public string FinancialProjectId { get; set; }
        
        public string Title { get; set; }

        public DateTime BoughtAt { get; set; }

    }

    public class UpdateReceiptsCommandHandler : IRequestHandler<UpdateReceiptsCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReceiptsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<Unit> Handle(UpdateReceiptsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Receipts.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            
            entity.Total = request.Total;
            entity.Title = request.Title;
            entity.BoughtAt = request.BoughtAt;
            
            if (!string.IsNullOrWhiteSpace(request.FinancialProjectId))
            {
                entity.FinancialProjectId = request.FinancialProjectId;
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}