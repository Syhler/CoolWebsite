using System.Threading.Tasks;

namespace Application.IntegrationTests.Common
{
    public class TestBase
    {
        public async Task TestSetUp()
        {
            await Testing.ResetState();
        }
    }
}