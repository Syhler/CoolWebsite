using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.Services;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems
{
    public class UpdateReceiptItemCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public int Count { get; set; }
        public double Price { get; set; }
        public int ItemGroup { get; set; }
        public List<UserDto> UserDtos { get; set; } = null!;
        public string FinancialProjectId { get; set; } = null!;
    }

    public class UpdateReceiptItemCommandHandler : IRequestHandler<UpdateReceiptItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReceiptItemCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
        }


        public async Task<Unit> Handle(UpdateReceiptItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ReceiptItems
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ReceiptItem), request.Id);
            }

            var records = _context.OweRecords
                .Where(x => x.FinancialProjectId == request.FinancialProjectId &&
                            x.OwedUserId == entity.CreatedBy)
                .ToList();


            //Add Receipt Item Cost for all users from request
            AddReceiptItemCostForRequestUsers(records, request);

            var usersToRemove = GetUserToRemove(entity, request.UserDtos);


            //Subtract Receipt Item Cost for removed users
            SubtractReceiptItemCostForRemovedUsers(usersToRemove, records, entity);

            //Subtract Receipt Item Cost for same users
            SubtractReceiptItemCostForSameUsers(records, entity, request);

            await CheckAndAddUsers(entity, request.Id, request.UserDtos);

            entity.Count = request.Count;
            entity.Price = request.Price;
            entity.ItemGroup = (ItemGroup) request.ItemGroup;

            if (usersToRemove.Any())
            {
                _context.ApplicationUserReceiptItems.RemoveRange(usersToRemove);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void SubtractReceiptItemCostForSameUsers(List<OweRecord> records, ReceiptItem entity,
            UpdateReceiptItemCommand request)
        {
            var sameUsers = entity.Users.Select(x => x.ApplicationUserId).ToList()
                .Intersect(request.UserDtos.Select(x => x.Id)).ToList();

            records.SubtractReceiptItemCost(sameUsers!, entity.Price, entity.Count, entity.Users.Count);
        }

        private void SubtractReceiptItemCostForRemovedUsers(List<ApplicationUserReceiptItem> usersToRemove,
            List<OweRecord> records, ReceiptItem entity)
        {
            var usersToRemoveStringList = usersToRemove
                .Select(x => x.ApplicationUserId)
                .ToList();

            records.SubtractReceiptItemCost(usersToRemoveStringList, entity.Price, entity.Count, entity.Users.Count);
        }

        private void AddReceiptItemCostForRequestUsers(List<OweRecord> records, UpdateReceiptItemCommand request)
        {
            var users = request.UserDtos
                .Select(x => x.Id)
                .ToList();

            records.AddReceiptItemCost(users!, request.Price, request.Count);
        }

        private async Task CheckAndAddUsers(ReceiptItem entity, string receiptId, List<UserDto> userDtos)
        {
            var newUsers = userDtos
                .Where(x => entity.Users.All(y => y.ApplicationUserId != x.Id))
                .Select(x => new ApplicationUserReceiptItem
                {
                    ApplicationUserId = x.Id,
                    ReceiptItemId = receiptId
                })
                .ToList();

            if (newUsers.Any())
            {
                await _context.ApplicationUserReceiptItems.AddRangeAsync(newUsers);
            }
        }

        private List<ApplicationUserReceiptItem> GetUserToRemove(ReceiptItem entity, List<UserDto> userDtos)
        {
            var usersToRemove = entity.Users
                .Where(x => userDtos.All(y => y.Id != x.ApplicationUserId))
                .ToList();

            return usersToRemove;
        }
    }
}