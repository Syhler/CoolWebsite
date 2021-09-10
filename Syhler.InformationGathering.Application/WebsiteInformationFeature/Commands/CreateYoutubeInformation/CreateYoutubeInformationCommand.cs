using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.Common.Interface;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateYoutubeInformation
{
    public class CreateYoutubeInformationCommand : IRequest<bool>
    {
        public string Url { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public bool IsInFocus { get; set; }
        public bool IsCurrentPage { get; set; }
        public bool IsPlayingAndNotFocus { get; set; }
        public bool IsPlayingAndNotFocusNorCurrentPage { get; set; }
    }
    
    public class CreateYoutubeInformationCommandHandler : IRequestHandler<CreateYoutubeInformationCommand, bool>
    {
        private readonly ICacheService _cacheService;
        private readonly IYoutubeInformationRepository _youtubeInformationRepository;

        public CreateYoutubeInformationCommandHandler(ICacheService cacheService, IYoutubeInformationRepository youtubeInformationRepository)
        {
            _cacheService = cacheService;
            _youtubeInformationRepository = youtubeInformationRepository;
        }

        public async Task<bool> Handle(CreateYoutubeInformationCommand request, CancellationToken cancellationToken)
        {
            var found = await _cacheService.TryAndGet<YoutubeResultModel>("DUMMYKEY", out var value);

            //Check if cached
            if (found)
            {
                //Use result Model

                return await _youtubeInformationRepository.Insert();
            }


            //Call youtube api if not cached
            var youtubeService = new YoutubeApiService();

            var information = await youtubeService.RequestInformation(request.Url);

            //Cache result for an hour
            await _cacheService.Save("DUMMYKEY", information);
            
            //Insert database
            return await _youtubeInformationRepository.Insert();
        }
    }
}