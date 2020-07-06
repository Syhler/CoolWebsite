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
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == request.Id);


            
            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            
            var records = _context.OweRecords.Where(x => x.OwedUserId == entity.CreatedBy && x.FinancialProjectId == entity.FinancialProjectId).ToList();

            
            entity.Location = request.Location;
            entity.DateVisited = request.Datevisited;
            entity.Note = request.Note;
            
            if (!string.IsNullOrWhiteSpace(request.FinancialProjectId))
            {
                entity.FinancialProjectId = request.FinancialProjectId;
            }


            var receiptItemsToRemove = entity.Items.Where(x => request.ItemDtos.All(y => y.Id != x.Id));
            
            
            await CreateReceiptItems(request, entity, records);
            
            _context.ReceiptItems.RemoveRange(receiptItemsToRemove);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }

        private async Task CreateReceiptItems(UpdateReceiptsCommand request, Receipt receipt, ICollection<OweRecord> records)
        {
            if (request.ItemDtos == null)
            {
                return;
            }
            
            foreach (var itemDto in request.ItemDtos)
            {
                var exist = receipt.Items.FirstOrDefault(x => x.Id == itemDto.Id);
                if (exist != null)
                {
                    UpdateOweRecords(exist, itemDto, records);
                    
                    exist.Count = itemDto.Count;
                    exist.Price = itemDto.Price;
                    exist.ItemGroup = (ItemGroup) itemDto.ItemGroup.Value;
                    var newUsers = CreateUserReceiptItemsList(itemDto.Users, itemDto.Id, exist.Users);
                    var usersToRemove = exist.Users.Where(x => itemDto.Users.All(y => y.Id != x.ApplicationUserId)).ToList();

                    if (usersToRemove.Any())
                    {
                        _context.ApplicationUserReceiptItems.RemoveRange(usersToRemove);
                    }

                    if (newUsers.Any())
                    {
                        await _context.ApplicationUserReceiptItems.AddRangeAsync(newUsers);
                    }
                    
                    
                    
                    _context.ReceiptItems.Update(exist);
                    
                    continue;
                }


                var receiptItemId = string.IsNullOrWhiteSpace(itemDto.Id) ? Guid.NewGuid().ToString() : itemDto.Id;

                var users = CreateUserReceiptItemsList(itemDto.Users, itemDto.Id);


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
                UpdateOweRecords(receiptItem, records);

                
                await _context.ReceiptItems.AddAsync(receiptItem);
            }
        }

        private void UpdateOweRecords(ReceiptItem oldItem, ReceiptItemDto newItem, ICollection<OweRecord> records)
        {

            foreach (var oldItemUser in oldItem.Users)
            {
                var record = records.FirstOrDefault(x => x.UserId == oldItemUser.ApplicationUserId);
                
                if (oldItem.Users.Count > 1)
                {
                    if (record != null)
                    {
                        record.Amount -= Math.Round(oldItem.Count * oldItem.Price/oldItem.Users.Count,2);
                    }

                }
                else
                {
                    if (record != null)
                    {
                        record.Amount -= Math.Round(oldItem.Count * oldItem.Price,2);
                    }
                }
            }
            
            foreach (var itemUser in newItem.Users)
            {
                var record = records.FirstOrDefault(x => x.UserId == itemUser.Id);
                
                if (newItem.Users.Count > 1)
                {
                    if (record != null)
                    {
                        record.Amount += Math.Round(newItem.Count * newItem.Price/newItem.Users.Count,2);
                    }

                }
                else
                {
                    if (record != null)
                    {
                        record.Amount += Math.Round(newItem.Count * newItem.Price,2);
                    }
                }
            }

        }
        
        private void UpdateOweRecords(ReceiptItem request, ICollection<OweRecord> records)
        {

            foreach (var user in request.Users)
            {
                
                var record = records.FirstOrDefault(x => x.UserId == user.ApplicationUserId);
                
                if (request.Users.Count > 1)
                {
                    if (record != null)
                    {
                        record.Amount += Math.Round(request.Count * request.Price/request.Users.Count,2);
                    }

                }
                else
                {
                    if (record != null)
                    {
                        record.Amount += Math.Round(request.Count * request.Price,2);
                    }
                }
            }
        }
        
        private List<ApplicationUserReceiptItem> CreateUserReceiptItemsList(ICollection<UserDto> userDtos, string receiptItemId)
        {
            var users = new List<ApplicationUserReceiptItem>();

            foreach (var user in userDtos)
            {
                users.Add(new ApplicationUserReceiptItem
                {
                    ApplicationUserId = user.Id,
                    ReceiptItemId = receiptItemId
                });
            }

            return users;
        }
        
        private List<ApplicationUserReceiptItem> CreateUserReceiptItemsList(ICollection<UserDto> userDtos, string receiptItemId, ICollection<ApplicationUserReceiptItem> skips)
        {
            var users = new List<ApplicationUserReceiptItem>();

            foreach (var user in userDtos)
            {
                if (skips.Any(x => x.ApplicationUserId == user.Id))
                {
                    continue;
                }
                
                users.Add(new ApplicationUserReceiptItem
                {
                    ApplicationUserId = user.Id,
                    ReceiptItemId = receiptItemId
                });
            }

            return users;
        }
    }
}