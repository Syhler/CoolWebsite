using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts
{
    public class UpdateReceiptCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public string? FinancialProjectId { get; set; }

        public string Location { get; set; } = null!;

        public string? Note { get; set; }

        public DateTime DateVisited { get; set; }

        public IList<ReceiptItemDto> ItemDtos { get; set; } = null!;
    }

    public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Receipts
                .Include(x => x.Items)
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == request.Id);


            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }

            var records = _context.OweRecords.Where(x =>
                x.OwedUserId == entity.CreatedBy && x.FinancialProjectId == entity.FinancialProjectId).ToList();


            entity.Location = request.Location;
            entity.DateVisited = request.DateVisited;
            entity.Note = request.Note;

            if (!string.IsNullOrWhiteSpace(request.FinancialProjectId))
            {
                entity.FinancialProjectId = request.FinancialProjectId;
            }


            //Remove items
            var receiptItemsToRemove = entity.Items.Where(x => request.ItemDtos.All(y => y.Id != x.Id)).ToList();
            
            RemoveReceiptItemFromOweRecord(receiptItemsToRemove, records);

            _context.ReceiptItems.RemoveRange(receiptItemsToRemove);
            
            //Add new items
            if (request.ItemDtos != null)
            {
                await ModifiesReceiptItem(request.ItemDtos, entity, records);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task ModifiesReceiptItem(IList<ReceiptItemDto> receiptItemDtos, Receipt receiptItem, ICollection<OweRecord> records)
        {
            foreach (var itemDto in receiptItemDtos)
            {
                var existingReceiptItem = receiptItem.Items.FirstOrDefault(x => x.Id == itemDto.Id);

                if (existingReceiptItem != null)
                {
                    
                    await UpdateReceiptItem(existingReceiptItem, itemDto, records);

                }
                else
                {
                    await CreateReceiptItems(receiptItem.Id, records, itemDto);
                }
            }
        }
        
        
        private void RemoveReceiptItemFromOweRecord(List<ReceiptItem> receiptItems, List<OweRecord> oweRecords)
        {
            foreach (var receiptItem in receiptItems)
            {
                foreach (var receiptItemUser in receiptItem.Users)
                {
                    var record = oweRecords.FirstOrDefault(x => x.UserId == receiptItemUser.ApplicationUserId);

                    if (record == null) continue;
                    
                    if (receiptItem.Users.Count > 1)
                    {
                        record.Amount -= Math.Round(receiptItem.Count * receiptItem.Price / receiptItem.Users.Count, 2);
                    }
                    else
                    {
                        record.Amount -= Math.Round(receiptItem.Count * receiptItem.Price, 2);
                    }

                }
                
            }
        }
        
        private async Task CreateReceiptItems(string receiptId, ICollection<OweRecord> records, ReceiptItemDto newItem)
        {
            var receiptItemId = string.IsNullOrWhiteSpace(newItem.Id) ? Guid.NewGuid().ToString() : newItem.Id;

            var users = CreateUserReceiptItemsList(newItem.Users, newItem.Id);


            //add item
            var receiptItem = new ReceiptItem
            {
                Id = receiptItemId,
                Count = newItem.Count,
                ItemGroup = newItem.ItemGroup != null ? (ItemGroup) newItem.ItemGroup.Value : ItemGroup.Unknown,
                Price = newItem.Price,
                ReceiptId = receiptId,
                Users = users,
                Name = "Receipt"
            };
        
            UpdateOweRecords(receiptItem, records);


            await _context.ReceiptItems.AddAsync(receiptItem);
        }

        private async Task UpdateReceiptItem(ReceiptItem receiptItem, ReceiptItemDto receiptItemDto, ICollection<OweRecord> records)
        {
            UpdateOweRecords(receiptItem, receiptItemDto, records);
            
            receiptItem.Count = receiptItemDto.Count;
            receiptItem.Price = receiptItemDto.Price;
            receiptItem.ItemGroup = receiptItemDto.ItemGroup != null ? (ItemGroup) receiptItemDto.ItemGroup.Value : ItemGroup.Unknown;

            var newUsers =
                CreateUserReceiptItemsListWithSkips(receiptItemDto.Users, receiptItemDto.Id, receiptItem.Users);

            //Gets all users there isn't on the receiptItem anymore
            var usersToRemove = receiptItem.Users.Where(x => receiptItemDto.Users.All(y => y.Id != x.ApplicationUserId))
                .ToList();

            if (usersToRemove.Any())
            {
                _context.ApplicationUserReceiptItems.RemoveRange(usersToRemove);
            }

            if (newUsers.Any())
            {
                await _context.ApplicationUserReceiptItems.AddRangeAsync(newUsers);
            }


            //_context.ReceiptItems.Update(receiptItem);
        }

        private void UpdateOweRecords(ReceiptItem oldItem, ReceiptItemDto newItem, ICollection<OweRecord> records)
        {
           
            RemoveOweAmountFromOldItems(oldItem, records);

            
            AddOweAmountForNewItem(newItem, records);
          
        }

        private void AddOweAmountForNewItem(ReceiptItemDto newItem, ICollection<OweRecord> records)
        {
            foreach (var itemUser in newItem.Users)
            {
                var record = records.FirstOrDefault(x => x.UserId == itemUser.Id);

                if (record == null) continue;

                if (newItem.Users.Count > 1)
                {
                    record.Amount += Math.Round(newItem.Count * newItem.Price / newItem.Users.Count, 2);
                }
                else
                {
                    record.Amount += Math.Round(newItem.Count * newItem.Price, 2);
                }
            }
        }

        private void RemoveOweAmountFromOldItems(ReceiptItem oldItem, ICollection<OweRecord> records)
        {
            foreach (var oldItemUser in oldItem.Users)
            {
                var record = records.FirstOrDefault(x => x.UserId == oldItemUser.ApplicationUserId);
                if (record == null) continue;

                if (oldItem.Users.Count > 1)
                {
                    record.Amount -= Math.Round(oldItem.Count * oldItem.Price / oldItem.Users.Count, 2);
                }
                else
                {
                    record.Amount -= Math.Round(oldItem.Count * oldItem.Price, 2);
                }
            }
        }


        private void UpdateOweRecords(ReceiptItem request, ICollection<OweRecord> records)
        {
            foreach (var user in request.Users)
            {
                var record = records.FirstOrDefault(x => x.UserId == user.ApplicationUserId);

                if (record == null) continue;

                if (request.Users.Count > 1)
                {
                    record.Amount += Math.Round(request.Count * request.Price / request.Users.Count, 2);
                }
                else
                {
                    record.Amount += Math.Round(request.Count * request.Price, 2);
                }
            }
        }

        private List<ApplicationUserReceiptItem> CreateUserReceiptItemsList(
            ICollection<UserDto>? userDtos,
            string? receiptItemId)
        {
            var users = new List<ApplicationUserReceiptItem>();

            if (userDtos == null) return users;
            
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

        private List<ApplicationUserReceiptItem> CreateUserReceiptItemsListWithSkips(ICollection<UserDto>? userDtos,
            string? receiptItemId, ICollection<ApplicationUserReceiptItem> skips)
        {
            var users = new List<ApplicationUserReceiptItem>();

            if (userDtos == null) return users;
            
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