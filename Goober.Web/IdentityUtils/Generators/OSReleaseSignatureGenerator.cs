using Goober.Web.IdentityUtils.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Goober.Web.IdentityUtils.Generators
{
    internal class OSReleaseSignatureGenerator : FileSignatureGeneratorBase
    {
        private const string nameGroup = "name";
        private const string valueGroup = "value";

        private readonly string _pattern = $@"(?'{nameGroup}'[A-Z][A-Z_0-9]+)=""?(?'{valueGroup}'[^""\n]*)""?";

        internal OSReleaseSignatureGenerator(string filePath, string separator)
            : base(filePath, separator)
        {
        }

        protected internal override string GetSignature()
        {
            var osRelease = GetModel();
            var namePart = !string.IsNullOrEmpty(osRelease.Name)
                ? osRelease.Name
                : osRelease.Id;

            var versionPart = !string.IsNullOrEmpty(osRelease.Version)
                ? osRelease.Version
                : osRelease.VersionId;

            if (string.IsNullOrEmpty(namePart) && string.IsNullOrEmpty(versionPart))
                return null;

            var creationTimePart = fileInfo.CreationTime.ToString("yyyyMMddHHmmss");

            var allParts = new[] { namePart, versionPart, creationTimePart }
                .Where(x => !string.IsNullOrEmpty(x));

            var result = string.Join(signatureSeparator, allParts);

            return result;
        }

        private OSReleaseModel GetModel()
        {
            var fileContent = File.ReadAllText(filePath);
            var matches = Regex.Matches(fileContent, _pattern);
            var nameAndValue = matches
                .Cast<Match>()
                .Where(match => match.Success)
                .Select(match =>
                {
                    var fieldName = match.Groups[nameGroup]?.Value ?? string.Empty;
                    var fieldValue = match.Groups[valueGroup]?.Value ?? string.Empty;

                    (string Name, string Value) result =
                    (
                        Name: fieldName,
                        Value: fieldValue
                    );

                    return result;
                })
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault().Value);

            var osRelease = new OSReleaseModel
            {
                PrettyName = nameAndValue.GetValueOrDefault("PRETTY_NAME", string.Empty),
                Name = nameAndValue.GetValueOrDefault("NAME", string.Empty),
                VersionId = nameAndValue.GetValueOrDefault("VERSION_ID", string.Empty),
                Version = nameAndValue.GetValueOrDefault("VERSION", string.Empty),
                VersionCodename = nameAndValue.GetValueOrDefault("VERSION_CODENAME", string.Empty),
                Id = nameAndValue.GetValueOrDefault("ID", string.Empty),
                HomeUrl = nameAndValue.GetValueOrDefault("HOME_URL", string.Empty),
                SupportUrl = nameAndValue.GetValueOrDefault("SUPPORT_URL", string.Empty),
                BugReportUrl = nameAndValue.GetValueOrDefault("BUG_REPORT_URL", string.Empty),
            };

            return osRelease;
        }
    }
}
