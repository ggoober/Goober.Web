using Goober.Web.IdentityUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Goober.Web.IdentityUtils.Converters
{
    internal class ProductInfoModelJsonConverter : JsonConverter<ProductInfoInternalModel>
    {
        public override ProductInfoInternalModel ReadJson(
            JsonReader reader,
            Type objectType,
            ProductInfoInternalModel existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            JObject jobject = JObject.Load(reader);
            if (jobject is null)
                return null;

            JObject licJson = jobject.GetValueOrDefault<JObject>(
                key: "lic",
                defaultValue: null,
                stringComparison: StringComparison.OrdinalIgnoreCase);
            var licenseInfoModel = ReadLicenseInfoJson(licJson);

            string productName = jobject.GetValueOrDefault(
                key: "productName",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var model = new ProductInfoInternalModel(
                productName: productName,
                licenseInfo: licenseInfoModel
            );

            return model;
        }

        public override void WriteJson(
            JsonWriter writer,
            ProductInfoInternalModel value,
            JsonSerializer serializer
        )
        {
            if (value is null)
                return;

            JObject lic = null;
            if (value.LicenseInfo != null)
            {
                lic = new JObject();
                lic["id"] = value.LicenseInfo.Id;
                lic["version"] = value.LicenseInfo.Version;
                lic["customer"] = value.LicenseInfo.Customer;
                lic["issuedAt"] = value.LicenseInfo.IssuedAt;
                lic["exp"] = value.LicenseInfo.ExpireDate;
                lic["licenseAuthor"] = value.LicenseInfo.LicenseAuthor;
                lic["fileName"] = value.LicenseInfo.ExpireDate;
                lic["comment"] = value.LicenseInfo.Comment;
                lic["isPerpetual"] = value.LicenseInfo.IsPerpetual;
            }

            JObject product = new JObject();
            product["productName"] = value.ProductName;
            product["lic"] = lic;

            product.WriteTo(writer);
        }

        private static LicenseInfoInternalModel ReadLicenseInfoJson(JObject jobject)
        {
            if (jobject is null)
                return null;

            var version = jobject.GetValueOrDefault(
                key: "version",
                defaultValue: 5,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var customer = jobject.GetValueOrDefault(
                key: "customer",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var issuedAt = jobject.GetValueOrDefault(
                key: "issuedAt",
                defaultValue: DateTime.MinValue,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var expireDate = jobject.GetValueOrDefault(
                key: "exp",
                defaultValue: DateTime.MinValue,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var licenseAuthor = jobject.GetValueOrDefault(
                key: "licenseAuthor",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var fileName = jobject.GetValueOrDefault(
                key: "fileName",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var comment = jobject.GetValueOrDefault(
                key: "comment",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var guidString = jobject.GetValue("id", StringComparison.OrdinalIgnoreCase).Value<string>();
            var id = Guid.TryParse(guidString, out var guid)
                ? guid
                : Guid.Empty;

            var isPerpetual = jobject.GetValueOrDefault(
                key: "isPerpetual",
                defaultValue: false,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var licenseInfoModel = new LicenseInfoInternalModel(
                id: id,
                version: version,
                customer: customer,
                issuedAt: issuedAt,
                expireDate: expireDate,
                licenseAuthor: licenseAuthor,
                fileName: fileName,
                comment: comment,
                isPerpetual: isPerpetual
            );

            return licenseInfoModel;
        }
    }
}
