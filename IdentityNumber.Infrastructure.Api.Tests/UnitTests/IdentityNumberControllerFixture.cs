using System;
using System.Collections.Generic;
using IdentityNumber.Domain.Repositories;
using IdentityNumber.Domain.Services;
using IdentityNumber.Infrastructure.Api.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityNumber.Infrastructure.Api.Tests.UnitTests
{
    public class IdentityNumberControllerFixture : IDisposable
    {
        public IdentityNumberControllerFixture()
        {
            MockLogger = new Mock<ILogger<IdentityNumberController>>();
            MockValidIdentityNumberRepository = new Mock<IValidIdentityNumberRepository>();
            MockInvalidIdentityNumberRepository = new Mock<IInvalidIdentityNumberRepository>();
            MockIdentityNumberService = new Mock<IIdentityNumberService>();
        }

        public Mock<ILogger<IdentityNumberController>> MockLogger { get; private set; }

        public IConfiguration Configuration
        {
            get
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                    {{"UploadFileSizeLimitInMb", "5"}});

                return configBuilder.Build();
            }
        }

        public Mock<IValidIdentityNumberRepository> MockValidIdentityNumberRepository { get; private set; }

        public Mock<IInvalidIdentityNumberRepository> MockInvalidIdentityNumberRepository { get; private set; }

        public Mock<IIdentityNumberService> MockIdentityNumberService { get; private set; }

        public void Dispose()
        {
        }
    }
}
