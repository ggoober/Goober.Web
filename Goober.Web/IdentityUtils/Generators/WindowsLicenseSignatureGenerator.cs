using Goober.Web.IdentityUtils.Services;
using System;
using System.Linq;
using System.Management;

namespace Goober.Web.IdentityUtils.Generators
{
    internal class WindowsLicenseSignatureGenerator : LicenseSignatureGenerator
    {
        internal WindowsLicenseSignatureGenerator(SingnatureCryptoServiceBase cryptoService)
            : base(cryptoService)
        {
        }

        /// <inheritdoc/>
        protected internal sealed override string Generate()
        {
            var result = GetWindowsMachineLicenseId();
            return result;
        }

        private string GetWindowsMachineLicenseId()
        {
            var wmiProps = new[] { "Name", "Version", "UUID", "IdentifyingNumber" };
            var scope = new ManagementScope($@"\\{Environment.MachineName}\root\cimv2");
            var query = new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct");

            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                var possibleKeys =
                  from managementObject in searcher.Get().OfType<ManagementBaseObject>()
                  let internalValues = wmiProps.Select(key => managementObject[key].ToString())
                                               .Where(val => !string.IsNullOrEmpty(val))
                  select string.Join(SignatureSeparator, internalValues);
                return possibleKeys.FirstOrDefault() ?? "";
            }
        }
    }
}
