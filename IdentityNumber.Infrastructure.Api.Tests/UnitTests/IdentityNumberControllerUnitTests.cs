using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IdentityNumber.Domain.Entities;
using IdentityNumber.Domain.Services;
using IdentityNumber.Infrastructure.Api.Controllers;
using IdentityNumber.Infrastructure.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace IdentityNumber.Infrastructure.Api.Tests.UnitTests
{
    public class IdentityNumberControllerUnitTests : IClassFixture<IdentityNumberControllerFixture>
    {
        private readonly IdentityNumberControllerFixture _fixture;

        public IdentityNumberControllerUnitTests(IdentityNumberControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetValidNumbers_Test_ReturnsOk()
        {
            // Arrange
            _fixture.MockValidIdentityNumberRepository.Setup(r => r.GetAsync()).ReturnsAsync(GetValidIdentityNumbers());
            _fixture.MockIdentityNumberService.Setup(s => s.GetValidAsync())
                .ReturnsAsync(GetValidIdentityNumbers());
            var identityNumberService = new IdentityNumberService(_fixture.MockValidIdentityNumberRepository.Object,
                _fixture.MockInvalidIdentityNumberRepository.Object);
            var controller = new IdentityNumberController(_fixture.Configuration, identityNumberService);

            // Act
            var result = await controller.GetValidNumbersAsync();

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<List<ValidIdentityNumber>>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var value = Assert.IsType<List<ValidIdentityNumber>>(objectResult.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetInvalidNumbers_Test_ReturnsOk()
        {
            // Arrange
            _fixture.MockInvalidIdentityNumberRepository.Setup(r => r.GetAsync())
                .ReturnsAsync(GetInvalidIdentityNumbers());
            _fixture.MockIdentityNumberService.Setup(s => s.GetInvalidAsync())
                .ReturnsAsync(GetInvalidIdentityNumbers());
            var identityNumberService = new IdentityNumberService(_fixture.MockValidIdentityNumberRepository.Object,
                _fixture.MockInvalidIdentityNumberRepository.Object);
            var controller = new IdentityNumberController(_fixture.Configuration, identityNumberService);

            // Act
            var result = await controller.GetInvalidNumbersAsync();

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<List<InvalidIdentityNumber>>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var value = Assert.IsType<List<InvalidIdentityNumber>>(objectResult.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task PostNullIdentityNumbers_Test_ReturnsBadRequest()
        {
            // Arrange
            var identityNumberService = new IdentityNumberService(_fixture.MockValidIdentityNumberRepository.Object,
                _fixture.MockInvalidIdentityNumberRepository.Object);
            var controller = new IdentityNumberController(_fixture.Configuration, identityNumberService);

            // Act
            var result = await controller.ValidateInputAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostValidIdentityNumbers_Test_ReturnsOk()
        {
            // Arrange
            var identityNumberService = new IdentityNumberService(_fixture.MockValidIdentityNumberRepository.Object,
                _fixture.MockInvalidIdentityNumberRepository.Object);
            var controller = new IdentityNumberController(_fixture.Configuration,
                identityNumberService);

            // Act
            var result = await controller.ValidateInputAsync(GetIdentityNumbers());

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<IdentityNumberListModel>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var model = Assert.IsType<IdentityNumberListModel>(objectResult.Value);
            Assert.Single(model.ValidIdentityNumbers);
            Assert.Single(model.InvalidIdentityNumbers);
        }

        [Fact]
        public async Task PostUploadInvalidIdentityNumberFiles_Test_ReturnsBadRequest()
        {
            // Arrange
            var identityNumberService = new IdentityNumberService(_fixture.MockValidIdentityNumberRepository.Object,
                _fixture.MockInvalidIdentityNumberRepository.Object);
            var controller = new IdentityNumberController(_fixture.Configuration, identityNumberService);

            // Act
            var result = await controller.ValidateUploadFilesAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UploadValidIdentityNumberFiles_Test_ReturnsOk()
        {
            // Arrange
            var identityNumberService = new IdentityNumberService(_fixture.MockValidIdentityNumberRepository.Object,
                _fixture.MockInvalidIdentityNumberRepository.Object);
            var controller = new IdentityNumberController(_fixture.Configuration, identityNumberService);

            var fileMock = new Mock<IFormFile>();
            var content = "8501015800088" + Environment.NewLine + "85010" + Environment.NewLine;
            const string fileName = "IdentityNumbers.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            ms.SetLength(content.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Act
            var result = await controller.ValidateUploadFilesAsync(new List<IFormFile> {fileMock.Object});

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<IdentityNumberListModel>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var model = Assert.IsType<IdentityNumberListModel>(objectResult.Value);
            Assert.Single(model.ValidIdentityNumbers);
            Assert.Single(model.InvalidIdentityNumbers);
        }

        private static IEnumerable<ValidIdentityNumber> GetValidIdentityNumbers()
        {
            return new List<ValidIdentityNumber>
            {
                GetValidIdentityNumber()
            };
        }

        private static ValidIdentityNumber GetValidIdentityNumber()
        {
            return new ValidIdentityNumber
            {
                IdentityNumber = 8501015800088, BirthDate = new DateTime(1985, 1, 1), Gender = "Female",
                Citizenship = "South African"
            };
        }

        private static IEnumerable<InvalidIdentityNumber> GetInvalidIdentityNumbers()
        {
            return new List<InvalidIdentityNumber>
            {
                GetInvalidIdentityNumber()
            };
        }

        private static InvalidIdentityNumber GetInvalidIdentityNumber()
        {
            return new InvalidIdentityNumber
            {
                IdentityNumber = "85010", ReasonsFailed = "Identity Number has incorrect length."
            };
        }

        private static Tuple<IEnumerable<ValidIdentityNumber>, IEnumerable<InvalidIdentityNumber>>
            GetTupleIdentityNumbers()
        {
            return new Tuple<IEnumerable<ValidIdentityNumber>, IEnumerable<InvalidIdentityNumber>>(
                GetValidIdentityNumbers(), GetInvalidIdentityNumbers());
        }

        private static List<string> GetIdentityNumbers()
        {
            return new List<string>
            {
                "8501015800088",
                "85010"
            };
        }
    }
}
