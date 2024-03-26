using Microsoft.VisualStudio.TestPlatform.TestHost;
using NerdStore.WebApp.IntegrationTests.Config;

namespace NerdStore.WebApp.IntegrationTests
{
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;
        public UsuarioTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }
    }
}
