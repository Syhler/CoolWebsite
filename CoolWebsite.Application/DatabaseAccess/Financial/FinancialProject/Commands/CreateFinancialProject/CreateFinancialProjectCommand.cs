using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject
{
    public class CreateFinancialProjectCommand : IRequest<string>
    {
        public string Name { get; set; }
        public IList<ApplicationUser> Users { get; set; }
    }
    
    public class CreateFinancialProjectCommandHandler : IRequestHandler<CreateFinancialProjectCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService userService)
        {
            _context = context;
            _context.UserId = userService.UserID;
        }

        public async Task<string> Handle(CreateFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Financial.FinancialProject
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name
            };
            
            var users = new List<FinancialProjectApplicationUser>();

            foreach (var applicationUser in request.Users)
            {
                users.Add(new FinancialProjectApplicationUser
                {
                    FinancialProjectId = entity.Id,
                    UserId = applicationUser.Id
                });
            }

            entity.FinancialProjectApplicationUsers = users;
            

            await _context.FinancialProjects.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}