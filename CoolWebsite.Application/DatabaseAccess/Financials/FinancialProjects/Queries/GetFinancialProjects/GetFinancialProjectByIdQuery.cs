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
        public string ProjectId { get; set; } = null!;
    }

    public class GetFinancialProjectByIdQueryHandler 
        : IRequestHandler<GetFinancialProjectByIdQuery, FinancialProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;


        public GetFinancialProjectByIdQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService userService)
        {
            _mapper = mapper;
            _context = context;
            _context.UserId = userService.UserId;
            _currentUserService = userService;
        }

        public async Task<FinancialProjectDto> Handle(GetFinancialProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var entity = await _context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .ThenInclude(x => x.User)
                .Include(x => x.Receipts)
                .Include(x => x.OweRecords)
                .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(FinancialProject), request.ProjectId);
            }

            var mapped = _mapper.Map<FinancialProjectDto>(entity);
            
            if (mapped == null)
            {
                throw new NullReferenceException("Mapped object was returned as null");
            }
            
            mapped.Receipts = mapped.Receipts
                .Where(x => x.Deleted == null) //Removes deleted receipts from the mapped project
                .OrderByDescending(x => x.DateVisited)
                .ToList(); 

            var currentUserRecords = entity.OweRecords
                .Where(x => x.UserId == _currentUserService.UserId && x.FinancialProjectId == mapped.Id)
                .ToList();

            foreach (var mappedReceipt in mapped.Receipts)
            {
                if (_currentUserService.UserId != null)
                {
                    mappedReceipt.CurrentUserId = _currentUserService.UserId;
                }
            }
            
            foreach (var mappedUser in mapped.Users)
            {
                if (mappedUser.Id == _currentUserService.UserId) continue;

                //Gets record where current user owe mappedUser money
                var affectedRecord = currentUserRecords.FirstOrDefault(x => x.OwedUserId == mappedUser.Id);

                //Gets records where mapped owe currentUsers money
                var records = entity.OweRecords.FirstOrDefault(x => x.UserId == mappedUser.Id && x.OwedUserId == _currentUserService.UserId);

                if (records == null) continue;
                
                mappedUser.Owed = affectedRecord != null
                    ? Math.Round(affectedRecord.Amount - records.Amount, 2) 
                    : Math.Round(-records.Amount,2);
            }
            
            
            return mapped;
            
        }
    }
    
    
}