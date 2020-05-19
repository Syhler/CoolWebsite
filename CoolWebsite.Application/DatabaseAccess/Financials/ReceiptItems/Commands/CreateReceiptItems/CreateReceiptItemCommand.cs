using System;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Enums;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems
{
    public class CreateReceiptItemCommand : IRequest<string>
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public ItemGroup ItemGroup { get; set; }

        public string ReceiptId { get; set; }

    }

    public class CreateReceiptItemCommandHandler : IRequestHandler<CreateReceiptItemCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateReceiptItemCommandHandler(IApplicationDbContext context, ICurrentUserService service)
        {
            _context = context;
            _context.UserId = service.UserID;
        }


        public async Task<string> Handle(CreateReceiptItemCommand request, CancellationToken cancellationToken)
        {
            var receipt = await _context.Receipts.FindAsync(request.ReceiptId);

            if (receipt == null)
            {
                throw new ParentObjectNotFoundException(nameof(Receipt), request.ReceiptId);
            }
            
            var entity = new ReceiptItem
            {
                Name = request.Name,
                Count = request.Count,
                Price = request.Price,
                ItemGroup = request.ItemGroup,
                Id = Guid.NewGuid().ToString(),
                ReceiptId = request.ReceiptId
            };

            await _context.ReceiptItems.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);


            return entity.Id;
        }
    }
}