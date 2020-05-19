using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Queries;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries
{
    public class GetReceiptByIdQueryVm : IRequest<ReceiptVm>
    {
        public string ReceiptId { get; set; }
    }

    public class GetReceiptByIdQueryVmHandler : IRequestHandler<GetReceiptByIdQueryVm, ReceiptVm>
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


        public async Task<ReceiptVm> Handle(GetReceiptByIdQueryVm request, CancellationToken cancellationToken)
        {
            //TODO(NEEDS TO BE LOOKED AT)
            /*
            var entity = _context.Receipts
                .Include(x => x.Receptors)
                .FirstOrDefault(x => x.Id == request.ReceiptId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.ReceiptId);
            }
            
            foreach (var individualReceipt in entity.Receptors)
            {
                individualReceipt.User = await _service.GetUserById(individualReceipt.UserId);
            }

            return new ReceiptVm
            {
                IndividualReceipts = entity.Receptors.AsQueryable().ProjectTo<IndividualReceiptDto>(_mapper.ConfigurationProvider).ToList()
            };
            */
            return null;

        }
    }
}