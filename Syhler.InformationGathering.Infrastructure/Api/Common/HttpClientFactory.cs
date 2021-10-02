using System.Net.Http;
using Syhler.InformationGathering.Application.Common.Interface;

namespace Syhler.InformationGathering.Infrastructure.Api.Common
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient InstantiateHttpClient()
        {
            return new();
        }
    }
}