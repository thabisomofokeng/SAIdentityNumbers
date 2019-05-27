using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityNumber.Domain.Entities;
using IdentityNumber.Domain.Services;
using IdentityNumber.Infrastructure.Api.Helpers;
using IdentityNumber.Infrastructure.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IdentityNumber.Infrastructure.Api.Controllers
{
    [Route("api/identityNumbers")]
    [ApiController]
    public class IdentityNumberController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IIdentityNumberService _identityNumberService;

        public IdentityNumberController(IConfiguration configuration,
            IIdentityNumberService identityNumberService)
        {
            _configuration = configuration;
            _identityNumberService = identityNumberService;
        }

        [HttpGet("valid")]
        public async Task<ActionResult<List<ValidIdentityNumber>>> GetValidNumbersAsync()
        {
            var validIdentityNumbers = await _identityNumberService.GetValidAsync();

            return Ok(validIdentityNumbers);
        }

        [HttpGet("invalid")]
        public async Task<ActionResult<List<InvalidIdentityNumber>>> GetInvalidNumbersAsync()
        {
            var invalidIdentityNumbers = await _identityNumberService.GetInvalidAsync();

            return Ok(invalidIdentityNumbers);
        }

        [HttpPost("validateInput")]
        public async Task<ActionResult<IdentityNumberListModel>> ValidateInputAsync([FromBody] List<string> identityNumbers)
        {
            if (identityNumbers == null)
            {
                return BadRequest($"{nameof(identityNumbers)} is invalid");
            }

            var (validIdentityNumbers, invalidIdentityNumbers) =
                await _identityNumberService.ValidateAsync(identityNumbers);
            var model = new IdentityNumberListModel
            {
                ValidIdentityNumbers = validIdentityNumbers,
                InvalidIdentityNumbers = invalidIdentityNumbers
            };

            return Ok(model);
        }

        [HttpPost("validateUploadFiles")]
        public async Task<ActionResult<IdentityNumberListModel>> ValidateUploadFilesAsync(List<IFormFile> files)
        {
            if (files == null)
            {
                return BadRequest($"{nameof(files)} is invalid");
            }

            var fileSizeLimit = _configuration.GetValue("UploadFileSizeLimitInMb", long.MaxValue);
            var totalUploadFilesSize = files.Sum(formFile => Math.Round(formFile.Length / 1000000d, 2));
            var identityNumbers = new List<string>();

            if (totalUploadFilesSize > fileSizeLimit)
                return BadRequest(
                    $"Total files size of {totalUploadFilesSize} MB exceeds allowed limit of {fileSizeLimit} MB");

            foreach (var formFile in files)
            {
                identityNumbers.AddRange(await formFile.ReadLinesAsync());
            }

            if (FileHelpers.HasBinaryContent(string.Join("", identityNumbers)))
                return BadRequest("Uploaded file(s) not valid text file(s)");

            var (validIdentityNumbers, invalidIdentityNumbers) =
                await _identityNumberService.ValidateAsync(identityNumbers);
            var model = new IdentityNumberListModel
            {
                ValidIdentityNumbers = validIdentityNumbers,
                InvalidIdentityNumbers = invalidIdentityNumbers
            };

            return Ok(model);
        }

    }
}
