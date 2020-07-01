using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts
{
    public class UpdateReceiptsCommand : IRequest
    {
        public string Id { get; set; }
        public string FinancialProjectId { get; set; }
        
        public string Location { get; set; }

        public string Note { get; set; }
        
        public DateTime Datevisited { get; set; }

        public IList<ReceiptItemDto> ItemDtos { get; set; }

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
            var entity = _context.Receipts
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            
            entity.Location = request.Location;
            entity.DateVisited = request.Datevisited;
            entity.Note = request.Note;
            
            if (!string.IsNullOrWhiteSpace(request.FinancialProjectId))
            {
                entity.FinancialProjectId = request.FinancialProjectId;
            }


            var receiptItemsToRemove = entity.Items.Where(x => request.ItemDtos.All(y => y.Id != x.Id));
            
            
            await CreateReceiptItems(request, entity);
            
            _context.ReceiptItems.RemoveRange(receiptItemsToRemove);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }

        private async Task CreateReceiptItems(UpdateReceiptsCommand request, Receipt receipt)
        {
            if (request.ItemDtos == null)
            {
                return;
            }
            
            foreach (var itemDto in request.ItemDtos)
            {
                var exist = receipt.Items.FirstOrDefault(x => x.Id == itemDto.Id);
                if (exist != null) continue;

                var users = new List<ApplicationUserReceiptItem>();

                var receiptItemId = string.IsNullOrWhiteSpace(itemDto.Id) ? Guid.NewGuid().ToString() : itemDto.Id;
                
                foreach (var user in itemDto.Users)
                {
                    users.Add(new ApplicationUserReceiptItem
                    {
                        ApplicationUserId = user.Id,
                        ReceiptItemId = receiptItemId
                    });
                }
                
                //add item
                var receiptItem = new ReceiptItem
                {
                    Id = receiptItemId,
                    Count = itemDto.Count,
                    ItemGroup = (ItemGroup)itemDto.ItemGroup.Value,
                    Price = itemDto.Price,
                    ReceiptId = receipt.Id,
                    Users = users,
                    Name = "Receipt"
                };
                await _context.ReceiptItems.AddAsync(receiptItem);
            }
        }
    }
}