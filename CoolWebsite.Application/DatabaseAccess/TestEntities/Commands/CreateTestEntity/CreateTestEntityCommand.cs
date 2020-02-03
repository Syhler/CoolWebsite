using System;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity
{
    public class CreateTestEntityCommand : IRequest<string>
    {
        public string Name { get; set; }
        
        public class CreateTestEntityCommandHandler : IRequestHandler<CreateTestEntityCommand, string>
        {
            private readonly IApplicationDbContext _context;

            public CreateTestEntityCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateTestEntityCommand request, CancellationToken cancellationToken)
            {
                var entity = new TestEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name
                };

                _context.TestEntities.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}