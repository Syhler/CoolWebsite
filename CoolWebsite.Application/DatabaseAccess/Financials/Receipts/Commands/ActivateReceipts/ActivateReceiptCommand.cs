using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.ActivateReceipts
{
    public class ActivateReceiptCommand : IRequest
    {
        public string ReceiptId { get; set; } = null!;
    }

    public class ActivateReceiptCommandHandler : IRequestHandler<ActivateReceiptCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;


        public ActivateReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ActivateReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Receipts
                .Include(x => x.Items)
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == request.ReceiptId && x.DeletedByUserId == _currentUserService.UserId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.ReceiptId);
            }


            if (entity.Items != null)
            {
                var oweRecords = _context.OweRecords.Where(x => x.FinancialProjectId == entity.FinancialProjectId);

                foreach (var receiptItem in entity.Items)
                {
                    AddOweAmount(oweRecords, receiptItem, entity.FinancialProjectId);

                }

            }
            
            entity.Deleted = null;
            entity.DeletedByUserId = null;


            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }

        private void AddOweAmount(IQueryable<OweRecord> oweRecords, ReceiptItem receiptItem, string projectId)
        {
            foreach (var user in receiptItem.Users)
            {
                var oweRecord = oweRecords.FirstOrDefault(x => x.UserId == user.ApplicationUserId && x.OwedUserId == _currentUserService.UserId);

                if (oweRecord == null) continue;

                var transactions = _context.Transactions.Where(x => x.FinancialProjectId == projectId && x.FromUserId == user.ApplicationUserId && x.ToUserId == _currentUserService.UserId);

                
                if (receiptItem.Users.Count > 1)
                {
                    UpdateRecordAmountMultiplesUsers(oweRecord, receiptItem);
                }
                else
                {
                    UpdateRecordAmount(oweRecord, receiptItem);
                }

                if (!transactions.Any())
                {
                    oweRecord.Amount *= -1;
                }

            }
        }

        /*
        private double SubtractTransaction(string financialProjectId)
        {
            var transactions = 
        }
        */
        
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