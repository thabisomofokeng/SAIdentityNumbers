using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using IdentityNumber.Domain.Entities;
using IdentityNumber.Domain.Repositories;
using IdentityNumber.Infrastructure.Repositories.Csv.CsvHelperMappings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IdentityNumber.Infrastructure.Repositories.Csv
{
    public class ValidIdentityNumberCsvRepository : IValidIdentityNumberRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ValidIdentityNumberCsvRepository(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        private string FileDirectory => _hostingEnvironment.ContentRootPath;
        private string FileName => _configuration.GetSection("CsvStore:ValidIdentityNumberFileName").Value;

        public async Task<IEnumerable<ValidIdentityNumber>> GetAsync()
        {
            using (var streamReader = new StreamReader(Path.Combine(FileDirectory, FileName)))
            using (var csvReader = new CsvReader(streamReader))
            {
                csvReader.Configuration.RegisterClassMap<ValidIdentityNumberMap>();
                var records = csvReader.GetRecords<ValidIdentityNumber>().ToList();

                return await Task.FromResult(records);
            }
        }

        public async Task<ValidIdentityNumber> AddAsync(ValidIdentityNumber validIdentityNumber)
        {
            using (var file = File.Open(Path.Combine(FileDirectory, FileName), FileMode.Append))
            using (var streamWriter = new StreamWriter(file))
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                csvWriter.Configuration.RegisterClassMap<ValidIdentityNumberMap>();
                csvWriter.WriteRecord(validIdentityNumber);
                csvWriter.NextRecord();

                return await Task.FromResult(validIdentityNumber);
            }
        }
    }
}
