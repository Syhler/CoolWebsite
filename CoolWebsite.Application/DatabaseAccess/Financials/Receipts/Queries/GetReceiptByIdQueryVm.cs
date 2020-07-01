using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries
{
    public class GetReceiptByIdQueryVm : IRequest<ReceiptDto>
    {
        public string ReceiptId { get; set; }
    }

    public class GetReceiptByIdQueryVmHandler : IRequestHandler<GetReceiptByIdQueryVm, ReceiptDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _service;


        public GetReceiptByIdQueryVmHandler(IApplicationDbContext context, ICurrentUserService userService, IMapper mapper, IIdentityService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
            _context.UserId = userService.UserID;
        }


        public async Task<ReceiptDto> Handle(GetReceiptByIdQueryVm request, CancellationToken cancellationToken)
        {
            var entity = _context.Receipts
                .Include(x => x.Items)
                .Where(x => x.Id == request.ReceiptId);
            
            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.ReceiptId);
            }

            var mapped = entity.ProjectTo<ReceiptDto>(_mapper.ConfigurationProvider);

            return mapped.First();
        }
    }
}