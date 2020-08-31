using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.Services;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems
{
    public class DeleteReceiptItemCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public string FinancialProjectId { get; set; } = null!;
    }

    public class DeleteReceiptItemCommandHandler : IRequestHandler<DeleteReceiptItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteReceiptItemCommandHandler(IApplicationDbContext context, ICurrentUserService userService)
        {
            _context = context;
            _context.UserId = userService.UserId;
        }


        public async Task<Unit> Handle(DeleteReceiptItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.ReceiptItems
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ReceiptItem), request.Id);
            }

            var records = _context.OweRecords
                .Where(x => x.FinancialProjectId == request.FinancialProjectId && x.OwedUserId == entity.CreatedBy)
                .ToList();

            records.SubtractReceiptItemCost(
                entity.Users.Select(x => x.ApplicationUserId).ToList(),
                entity.Price,
                entity.Count,
                entity.Users.Count);

            //delete

            _context.ReceiptItems.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}