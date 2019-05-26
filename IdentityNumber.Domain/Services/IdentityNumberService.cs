using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IdentityNumber.Domain.Entities;
using IdentityNumber.Domain.Repositories;

namespace IdentityNumber.Domain.Services
{
    public class IdentityNumberService : IIdentityNumberService
    {
        private readonly IValidIdentityNumberRepository _validIdentityNumberRepository;
        private readonly IInvalidIdentityNumberRepository _invalidIdentityNumberRepository;

        public IdentityNumberService(IValidIdentityNumberRepository validIdentityNumberRepository,
            IInvalidIdentityNumberRepository invalidIdentityNumberRepository)
        {
            _validIdentityNumberRepository = validIdentityNumberRepository;
            _invalidIdentityNumberRepository = invalidIdentityNumberRepository;
        }

        public async Task<IEnumerable<InvalidIdentityNumber>> GetInvalidAsync()
        {
            return await _invalidIdentityNumberRepository.GetAsync();
        }

        public async Task<IEnumerable<ValidIdentityNumber>> GetValidAsync()
        {
            return await _validIdentityNumberRepository.GetAsync();
        }

        public async Task<Tuple<IEnumerable<ValidIdentityNumber>, IEnumerable<InvalidIdentityNumber>>> ProcessAsync(
            List<string> identityNumbers)
        {
            Validate(identityNumbers);

            var validIdentityNumbers = new List<ValidIdentityNumber>();
            var invalidIdentityNumbers = new List<InvalidIdentityNumber>();
            var tasks = new ConcurrentBag<string>(identityNumbers).Select(async identityNumber =>
            {
                if (string.IsNullOrWhiteSpace(identityNumber))
                    return;

                var reasonsFailed = new List<string>();

                if (identityNumber.Trim().Length != 13)
                {
                    reasonsFailed.Add("Identity Number has incorrect length.");
                }

                if (!long.TryParse(identityNumber, out var longIdentityNumber))
                {
                    reasonsFailed.Add("Identity Number has invalid format.");
                }

                if (reasonsFailed.Any())
                {
                    var invalidIdentityNumber = new InvalidIdentityNumber
                    {
                        IdentityNumber = identityNumber,
                        ReasonsFailed = string.Join(" | ", reasonsFailed)
                    };

                    await _invalidIdentityNumberRepository.AddAsync(invalidIdentityNumber);

                    return;
                }

                int.TryParse(identityNumber.Substring(0, 2), out var twoDigitYear);
                int.TryParse(identityNumber.Substring(2, 2), out var month);
                int.TryParse(identityNumber.Substring(4, 2), out var day);

                var fourDigitYear = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(twoDigitYear);
                var birthDate = new DateTime(fourDigitYear, month, day);

                if (birthDate.Year <= 0)
                {
                    reasonsFailed.Add("Year of birth is invalid");
                }

                if (birthDate.Month <= 0)
                {
                    reasonsFailed.Add("Month of birth is invalid");
                }

                if (birthDate.Day <= 0)
                {
                    reasonsFailed.Add("Day of birth is invalid");
                }

                //todo: check gender
                int.TryParse(identityNumber.Substring(6, 4), out var gender);

                //todo: check cs
                int.TryParse(identityNumber.Substring(10, 1), out var citizenship);

                //todo: perform luhn 
                int.TryParse(identityNumber.Substring(12, 1), out var checksum);

                if (reasonsFailed.Any())
                {
                    var invalidIdentityNumber = new InvalidIdentityNumber
                    {
                        IdentityNumber = identityNumber,
                        ReasonsFailed = string.Join(" | ", reasonsFailed)
                    };

                    await _invalidIdentityNumberRepository.AddAsync(invalidIdentityNumber);

                    invalidIdentityNumbers.Add(invalidIdentityNumber);

                    return;
                }

                var validIdentityNumber = new ValidIdentityNumber
                {
                    IdentityNumber = longIdentityNumber,
                    BirthDate = birthDate,
                    Gender = gender < 4999 ? "Female" : "Male",
                    Citizenship = citizenship == 0 ? "SA Citizen" : "Permanent Resident"
                };

                await _validIdentityNumberRepository.AddAsync(validIdentityNumber);

                validIdentityNumbers.Add(validIdentityNumber);
            });

            await Task.WhenAll(tasks);

            return new Tuple<IEnumerable<ValidIdentityNumber>, IEnumerable<InvalidIdentityNumber>>(validIdentityNumbers,
                invalidIdentityNumbers);
        }

        private static void Validate(IEnumerable<string> identityNumbers)
        {
            if (identityNumbers == null)
                throw new ArgumentNullException(nameof(identityNumbers), "Supplied object is not valid");
        }
    }
}
