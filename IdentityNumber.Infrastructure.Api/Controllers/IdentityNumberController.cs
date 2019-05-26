using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IdentityNumber.Domain.Entities;
using IdentityNumber.Domain.Services;
using IdentityNumber.Infrastructure.Api.Helpers;
using IdentityNumber.Infrastructure.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityNumber.Infrastructure.Api.Controllers
{
    [Route("api/identityNumbers")]
    [ApiController]
    public class IdentityNumberController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IIdentityNumberService _identityNumberService;

        public IdentityNumberController(IConfiguration configuration, ILogger logger,
            IIdentityNumberService identityNumberService)
        {
            _configuration = configuration;
            _logger = logger;
            _identityNumberService = identityNumberService;
        }

        [HttpGet("valid")]
        public async Task<ActionResult<List<ValidIdentityNumber>>> GetValidNumbersAsync()
        {
            try
            {
                var validIdentityNumbers = await _identityNumberService.GetValidAsync();

                return Ok(validIdentityNumbers);
            }
            catch (IOException e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("invalid")]
        public async Task<ActionResult<List<InvalidIdentityNumber>>> GetInvalidNumbersAsync()
        {
            try
            {
                var invalidIdentityNumbers = await _identityNumberService.GetInvalidAsync();

                return Ok(invalidIdentityNumbers);
            }
            catch (IOException e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IdentityNumberListModel>> PostAsync([FromBody] List<string> identityNumbers)
        {
            try
            {
                if (identityNumbers == null)
                {
                    return BadRequest($"{nameof(identityNumbers)} is invalid");
                }

                var (validIdentityNumbers, invalidIdentityNumbers) =
                    await _identityNumberService.ProcessAsync(identityNumbers);
                var model = new IdentityNumberListModel
                {
                    ValidIdentityNumbers = validIdentityNumbers,
                    InvalidIdentityNumbers = invalidIdentityNumbers
                };

                return Ok(model);
            }
            catch (IOException e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                //todo: log
                _logger.LogError($"Something went wrong: {e}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<IdentityNumberListModel>> UploadIdentityNumberFilesAsync(List<IFormFile> files)
        {
            try
            {
                if (files == null)
                {
                    return BadRequest($"{nameof(files)} is invalid");
                }

                var fileSizeLimit = _configuration.GetValue("UploadFileSizeLimitInMb", long.MaxValue);
                var identityNumbers = new List<string>();

                foreach (var formFile in files)
                {
                    var size = Math.Round(formFile.Length / 1000000d, 2);

                    if (size > fileSizeLimit)
                        return BadRequest(
                            $"File '{formFile.FileName}' size of {size} MB exceeded allowed limit of {fileSizeLimit} MB");

                    identityNumbers.AddRange(await formFile.ReadLinesAsync());
                }

                var (validIdentityNumbers, invalidIdentityNumbers) =
                    await _identityNumberService.ProcessAsync(identityNumbers);
                var model = new IdentityNumberListModel
                {
                    ValidIdentityNumbers = validIdentityNumbers,
                    InvalidIdentityNumbers = invalidIdentityNumbers
                };

                return Ok(model);
            }
            catch (IOException e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
