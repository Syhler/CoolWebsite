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
    public class DeleteReceiptCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteReceiptCommandHandler : IRequestHandler<DeleteReceiptCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<Unit> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
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
                   SubtractOweAmount(records, receiptItem);
                }
            }
           
            entity.Deleted = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void SubtractOweAmount(IQueryable<OweRecord> oweRecords, ReceiptItem receiptItem)
        {
            foreach (var user in receiptItem.Users)
            {
                var record = oweRecords.FirstOrDefault(x => x.UserId == user.ApplicationUserId && x.OwedUserId == _currentUserService.UserID);

                if (record == null) continue;
                        
                if (receiptItem.Users.Count > 1)
                {
                    UpdateRecordAmountMultiplesUsers(record, receiptItem);

                }
                else
                {
                    UpdateRecordAmount(record,receiptItem);
                }
            }

        }
        
        private void UpdateRecordAmount(OweRecord record, ReceiptItem receiptItem)
        {
            if (record.Amount < 0)
            {
                record.Amount += receiptItem.Count * receiptItem.Price;

            }
            else
            {
                record.Amount -= receiptItem.Count * receiptItem.Price;

            }
        }
        
        private void UpdateRecordAmountMultiplesUsers(OweRecord record, ReceiptItem receiptItem)
        {
            if (record.Amount < 0)
            {
                record.Amount += (receiptItem.Count * receiptItem.Price)/receiptItem.Users.Count;

            }
            else
            {
                record.Amount -= (receiptItem.Count * receiptItem.Price)/receiptItem.Users.Count;

            }
        }
    }
    
}