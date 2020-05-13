using System.Threading.Tasks;
using NUnit.Framework;

namespace Application.IntegrationTests.Common
{
    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await Testing.ResetState();
        }
    }
}