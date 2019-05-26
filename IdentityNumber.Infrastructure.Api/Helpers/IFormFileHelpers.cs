using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityNumber.Infrastructure.Api.Helpers
{
    public static class FileHelpers
    {
        public static async Task<List<string>> ReadLinesAsync(this IFormFile file)
        {
            var lines = new List<string>();

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                while (streamReader.Peek() >= 0)
                {
                    var text = await streamReader.ReadLineAsync();

                    lines.Add(text);
                }
            }

            if (HasBinaryContent(string.Join("", lines)))
                throw new InvalidOperationException($"{file.FileName} is not a valid text file");

            return lines;
        }

        private static bool HasBinaryContent(string content)
        {
            return content.Any(ch => char.IsControl(ch) && ch != '\r' && ch != '\n');
        }
    }
}
