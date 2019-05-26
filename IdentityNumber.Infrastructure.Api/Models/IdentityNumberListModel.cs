using System.Collections.Generic;
using IdentityNumber.Domain.Entities;

namespace IdentityNumber.Infrastructure.Api.Models
{
    public class IdentityNumberListModel
    {
        public IEnumerable<ValidIdentityNumber> ValidIdentityNumbers { get; set; }

        public IEnumerable<InvalidIdentityNumber> InvalidIdentityNumbers { get; set; }
    }
}
