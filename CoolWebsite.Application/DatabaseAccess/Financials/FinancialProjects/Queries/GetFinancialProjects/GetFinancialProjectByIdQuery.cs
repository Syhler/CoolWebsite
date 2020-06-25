﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetFinancialProjectByIdQuery : IRequest<FinancialProjectDto>
    {
        public string ProjectId { get; set; }
    }

    public class GetFinancialProjectByIdQueryHandler 
        : IRequestHandler<GetFinancialProjectByIdQuery, FinancialProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;


        public GetFinancialProjectByIdQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService userService, IIdentityService identityService)
        {
            _mapper = mapper;
            _context = context;
            _context.UserId = userService.UserID;
            _currentUserService = userService;
            _identityService = identityService;
        }

        public async Task<FinancialProjectDto> Handle(GetFinancialProjectByIdQuery request, CancellationToken cancellationToken)
        {


            var entity = _context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include(x => x.Receipts)
                .Where(x => x.Id == request.ProjectId);

            if (entity.ToList().Count == 0)
            {
                throw new NotFoundException(nameof(FinancialProject), request.ProjectId);
            }

            var mapped = entity.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).FirstOrDefault();

            if (mapped == null)
            {
                throw new NullReferenceException("Mapped object was returned as null");
            }
            
            mapped.Receipts = mapped.Receipts.Where(x => x.Deleted == null).ToList();

            foreach (var user in mapped.Users)
            {
                if (user.Id == _currentUserService.UserID)
                {
                    continue;
                }
                
                foreach (var receipt in mapped.Receipts)
                {
                    
                    var applicationUser = await _identityService.GetUserById(receipt.CreatedByUserId);
                    receipt.CreatedByDto = _mapper.Map<ApplicationUser, UserDto>(applicationUser);
                    
                    foreach (var item in receipt.Items)
                    {
                        var userExist = item.Users.FirstOrDefault(x => x.Id == user.Id);

                        if (userExist == null)
                        {
                            var loggedInUserReceiptItem =
                                item.Users.FirstOrDefault(x => x.Id == _currentUserService.UserID);
                            
                            if (user.Id == receipt.CreatedByUserId && loggedInUserReceiptItem != null)
                            {

                                var userCount = getUserCount(item, receipt);
                                if (userCount > 0)
                                {
                                    user.Owed += item.Total/userCount;

                                }
                                else
                                {
                                    user.Owed += item.Total;

                                }
                            }
                        }
                        else
                        {
                            if (receipt.CreatedByUserId == _currentUserService.UserID)
                            {
                                var userCount = getUserCount(item, receipt);
                                if (userCount > 0)
                                {
                                    user.Owed -= item.Total/userCount;

                                }
                                else
                                {
                                    user.Owed -= item.Total;

                                }
                            }
                            else
                            {
                                if (user.Id == receipt.CreatedByUserId)
                                {
                                    var userCount = getUserCount(item, receipt);
                                    if (userCount > 0)
                                    {
                                        user.Owed += item.Total/userCount;
                                    }
                                    else
                                    {
                                        user.Owed += item.Total;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
            
            return mapped;
            
            
            
            
            
            //better optimized. but couldnt get it to work.....
            /*

            var entity = _context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Where(x => x.Id == request.ProjectId)
                .Select(x => new
                {
                    x,
                    Receipts = x.Receipts.Where(x => x.Deleted == null).ToList()
                });
            
            
            if (entity == null)
            {
                throw new NotFoundException(nameof(FinancialProject), request.ProjectId);
            }

            /*
            var query = new List<FinancialProject>
            {
                entity.x
            }.AsQueryable();
            

            var mapped = _mapper.Map<FinancialProject, FinancialProjectDto>(entity.x);
            var mapped = entity.Select(x => x.x).ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).FirstOrDefault();
            */

            
            return mapped;
        }

        private int getUserCount(ReceiptItemDto item, ReceiptDto receipt)
        {
            var userCount = item.Users.Count;
            if (item.Users.FirstOrDefault(x => x.Id == receipt.CreatedByUserId) != null)
            {
                userCount--;
            }

            return userCount;
        }
    }
    
    
}