using System;

namespace IdentityNumber.Domain.Entities
{
    public class ValidIdentityNumber
    {
        public long IdentityNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Citizenship { get; set; }
    }
}
