using System.Net.Http;

namespace Syhler.InformationGathering.Application.Common.Interface
{
    public interface IHttpClientFactory
    {
        public HttpClient InstantiateHttpClient();
    }
}