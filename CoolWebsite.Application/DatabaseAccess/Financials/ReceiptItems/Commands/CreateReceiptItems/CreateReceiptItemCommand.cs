﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Domain.Enums;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems
{
    public class CreateReceiptItemCommand : IRequest<string>
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public int ItemGroup { get; set; }
        public string ReceiptId { get; set; }

        public List<string> UsersId { get; set; }

    }

    public class CreateReceiptItemCommandHandler : IRequestHandler<CreateReceiptItemCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identity;

        public CreateReceiptItemCommandHandler(IApplicationDbContext context, ICurrentUserService service, IIdentityService identity)
        {
            _context = context;
            _identity = identity;
            _context.UserId = service.UserID;
        }


        public async Task<string> Handle(CreateReceiptItemCommand request, CancellationToken cancellationToken)
        {
            var receipt = await _context.Receipts.FindAsync(request.ReceiptId);

            if (receipt == null)
            {
                throw new ParentObjectNotFoundException(nameof(Receipt), request.ReceiptId);
            }

            var id = Guid.NewGuid().ToString();


            var users = new List<ApplicationUserReceiptItem>();

            foreach (var user in request.UsersId)
            {
                users.Add(new ApplicationUserReceiptItem
                {
                    ApplicationUserId = user,
                    ReceiptItemId = id
                });
            }
            
            var entity = new ReceiptItem
            {
                Id = id,
                Name = request.Name,
                Count = request.Count,
                Price = request.Price,
                ItemGroup = (ItemGroup)request.ItemGroup,
                ReceiptId = request.ReceiptId,
                Users = users
            };

            await _context.ReceiptItems.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);


            return entity.Id;
        }

       
    }
}