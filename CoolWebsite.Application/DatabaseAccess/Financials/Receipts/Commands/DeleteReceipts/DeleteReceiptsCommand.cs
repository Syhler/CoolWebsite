using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var entity = _context.Receipts
                .Include(x => x.Items)
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            
            //remove from OwedRecords


            if (entity.Items != null)
            {
                var records = _context.OweRecords.Where(x => x.FinancialProjectId == entity.FinancialProjectId);

                foreach (var receiptItem in entity.Items)
                {
                    foreach (var user in receiptItem.Users)
                    {
                
                        var record = records.FirstOrDefault(x => x.UserId == user.ApplicationUserId);
                
                        if (receiptItem.Users.Count > 1)
                        {
                            if (record != null)
                            {
                                record.Amount -= (receiptItem.Count * receiptItem.Price)/receiptItem.Users.Count;
                            }

                        }
                        else
                        {
                            if (record != null)
                            {
                                record.Amount -= receiptItem.Count * receiptItem.Price;
                            }
                        }
                    }
                }
            }
            
           
            
            
            
            entity.Deleted = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}