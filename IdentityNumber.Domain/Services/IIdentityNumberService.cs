using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityNumber.Domain.Entities;

namespace IdentityNumber.Domain.Services
{
    public interface IIdentityNumberService
    {
        Task<IEnumerable<ValidIdentityNumber>> GetValidAsync();

        Task<IEnumerable<InvalidIdentityNumber>> GetInvalidAsync();

        Task<Tuple<IEnumerable<ValidIdentityNumber>, IEnumerable<InvalidIdentityNumber>>> ValidateAsync(
            List<string> identityNumbers);
    }
}
