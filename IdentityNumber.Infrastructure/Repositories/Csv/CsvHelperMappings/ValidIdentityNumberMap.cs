using CsvHelper.Configuration;
using IdentityNumber.Domain.Entities;

namespace IdentityNumber.Infrastructure.Repositories.Csv.CsvHelperMappings
{
    public class ValidIdentityNumberMap : ClassMap<ValidIdentityNumber>
    {
        public ValidIdentityNumberMap()
        {
            Map(m => m.IdentityNumber).Name("IdentityNumber");
            Map(m => m.BirthDate).Name("BirthDate");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Citizenship).Name("Citizenship");
        }
    }
}
