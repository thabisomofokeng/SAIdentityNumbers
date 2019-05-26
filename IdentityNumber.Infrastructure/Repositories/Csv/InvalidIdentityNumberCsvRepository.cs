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
    public class InvalidIdentityNumberCsvRepository : IInvalidIdentityNumberRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public InvalidIdentityNumberCsvRepository(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        private string FileDirectory => _hostingEnvironment.ContentRootPath;
        private string FileName => _configuration.GetSection("CsvStore:InvalidIdentityNumberFileName").Value;

        public async Task<IEnumerable<InvalidIdentityNumber>> GetAsync()
        {
            using (var streamReader = new StreamReader(Path.Combine(FileDirectory, FileName)))
            using (var csvReader = new CsvReader(streamReader))
            {
                csvReader.Configuration.RegisterClassMap<InvalidIdentityNumberMap>();
                var records = csvReader.GetRecords<InvalidIdentityNumber>().ToList();

                return await Task.FromResult(records);
            }
        }

        public async Task<InvalidIdentityNumber> AddAsync(InvalidIdentityNumber invalidIdentityNumber)
        {
            using (var file = File.Open(Path.Combine(FileDirectory, FileName), FileMode.Append))
            using (var streamWriter = new StreamWriter(file))
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                csvWriter.Configuration.RegisterClassMap<InvalidIdentityNumberMap>();
                csvWriter.WriteRecord(invalidIdentityNumber);
                csvWriter.NextRecord();

                return await Task.FromResult(invalidIdentityNumber);
            }
        }
    }
}
