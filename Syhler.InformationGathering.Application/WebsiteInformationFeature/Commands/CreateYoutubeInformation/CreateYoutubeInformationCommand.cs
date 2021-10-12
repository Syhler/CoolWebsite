using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.Common.Interface;
using Syhler.InformationGathering.Application.Services;
using Syhler.InformationGathering.Application.Services.Interface;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Application.Services.YoutubeApi.Enum;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateYoutubeInformation
{
    public class CreateYoutubeInformationCommand : IRequest<bool>
    {
        public string Url { get; set; } = null!;
        public DateTime TimeVisited { get; set; }
        public bool IsInFocus { get; set; }
        public bool IsCurrentPage { get; set; }
        public bool IsPlayingAndNotFocus { get; set; }
        public bool IsPlayingAndNotFocusNorCurrentPage { get; set; }
    }
    
    public class CreateYoutubeInformationCommandHandler : IRequestHandler<CreateYoutubeInformationCommand, bool>
    {
        private readonly ICacheService _cacheService;
        private readonly IYoutubeInformationRepository _youtubeInformationRepository;
        private readonly IYoutubeApiService _youtubeApiService;
        private readonly IWebsiteTypeService _websiteTypeService;

        public CreateYoutubeInformationCommandHandler(ICacheService cacheService,
            IYoutubeInformationRepository youtubeInformationRepository, 
            IYoutubeApiService youtubeApiService, IWebsiteTypeService websiteTypeService)
        {
            _cacheService = cacheService;
            _youtubeInformationRepository = youtubeInformationRepository;
            _youtubeApiService = youtubeApiService;
            _websiteTypeService = websiteTypeService;
        }

        public async Task<bool> Handle(CreateYoutubeInformationCommand request, CancellationToken cancellationToken)
        {
            var key = nameof(CreateYoutubeInformationCommand) + $"_{request.Url}";
            
            var found = await _cacheService.TryAndGet<YoutubeResultModel>(key, out var value);

            //Check if cached
            if (found)
            {
                var cacheModel = CreateModel(request, value);
                //Use result Model
                return await _youtubeInformationRepository.Insert(cacheModel);
            }

            //Call youtube api if not cached
            var information = await _youtubeApiService.RequestInformation(request.Url);

            //Cache result for an hour
            await _cacheService.Save(key, information);

            var model = CreateModel(request, information);
            
            //Insert database
            return await _youtubeInformationRepository.Insert(model);
        }

        private YoutubeInformation CreateModel(CreateYoutubeInformationCommand request, YoutubeResultModel model)
        {
            var videoType = _websiteTypeService.GetTypeFromYoutube(model);

            var category = ((YoutubeCategory) model.CategoryId).ToString();

            return new YoutubeInformation(
                WebsiteId.NewId(), 
                request.Url, 
                request.IsCurrentPage,
                request.IsInFocus, 
                videoType,
                request.TimeVisited,
                category, 
                model.Title, 
                request.Url, 
                model.ChannelTitle,
                request.IsPlayingAndNotFocusNorCurrentPage,
                request.IsPlayingAndNotFocus);

        }
    }
}