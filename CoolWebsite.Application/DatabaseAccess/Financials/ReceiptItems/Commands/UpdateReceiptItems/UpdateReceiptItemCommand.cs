using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems
{
    public class UpdateReceiptItemCommand : IRequest
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public int ItemGroup { get; set; }

        public string ReceiptItemId { get; set; }
    }
    
    public class UpdateReceiptItemCommandHandler : IRequestHandler<UpdateReceiptItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReceiptItemCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }


        public async Task<Unit> Handle(UpdateReceiptItemCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.ReceiptItems.FindAsync(request.ReceiptItemId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ReceiptItem), request.ReceiptItemId);
            }

            
            
            entity.Count = request.Count;
            entity.Price = request.Price;
            entity.ItemGroup = (ItemGroup)request.ItemGroup;
            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}