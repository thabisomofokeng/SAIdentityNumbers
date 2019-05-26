using CsvHelper.Configuration;
using IdentityNumber.Domain.Entities;

namespace IdentityNumber.Infrastructure.Repositories.Csv.CsvHelperMappings
{
    public class InvalidIdentityNumberMap : ClassMap<InvalidIdentityNumber>
    {
        public InvalidIdentityNumberMap()
        {
            Map(m => m.IdentityNumber).Name("IdentityNumber");
            Map(m => m.ReasonsFailed).Name("ReasonsFailed");
        }
    }
}
