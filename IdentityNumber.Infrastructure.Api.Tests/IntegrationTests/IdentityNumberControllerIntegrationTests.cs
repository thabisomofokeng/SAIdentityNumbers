using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdentityNumber.Infrastructure.Api.Tests.IntegrationTests
{
    [TestClass]
    public class IdentityNumberControllerIntegrationTests : IDisposable
    {
        private readonly HttpClient _client;

        [TestMethod]
        public void Post_ValidIdentityNumbers_ReturnsOK()
        {
            // Arrange


            // Act


            // Assert

        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
