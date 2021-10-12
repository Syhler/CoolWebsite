using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Domain.Enums;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateWebsiteInformation
{
    public class CreateTabWebsiteInformationCommand : IRequest<bool>
    {
        public CreateTabWebsiteInformationCommand(IEnumerable<string> urls, DateTime timeVisited)
        {
            Urls = urls;
            TimeVisited = timeVisited;
        }

        public IEnumerable<string> Urls { get; }
        public DateTime TimeVisited { get; }
    }

    public class CreateTabWebsiteInformationCommandHandler : IRequestHandler<CreateTabWebsiteInformationCommand, bool>
    {
        private readonly IWebsiteInformationRepository _websiteInformationRepository;

        public CreateTabWebsiteInformationCommandHandler(IWebsiteInformationRepository websiteInformationRepository)
        {
            _websiteInformationRepository = websiteInformationRepository;
        }

        public async Task<bool> Handle(CreateTabWebsiteInformationCommand request, CancellationToken cancellationToken)
        {
            var collection = new List<WebsiteInformation>();
            
            foreach (var requestUrl in request.Urls)
            {
                var model = new WebsiteInformation(
                    WebsiteId.NewId(), 
                    requestUrl,
                    false,
                    false,
                    WebsiteInformationType.UNKOWN,
                    request.TimeVisited);
                
                collection.Add(model);
            }

            return await _websiteInformationRepository.InsertRangeAsync(collection);
        }
    }
}