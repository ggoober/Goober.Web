using Goober.Web.IdentityUtils.Models;
using System;

namespace Goober.Web.IdentityUtils.Helpers
{
    internal static class IdentityUtilsHelper
    {
        internal static IndusoftProduct GetIndusoftProduct(object inputProductId)
        {
            var product = GetIndusoftProductByAssemblyName();
            product ??= GetIndusoftProductById(inputProductId);
            return product;
        }

        internal static IndusoftProduct GetIndusoftProductById(object inputProductId)
        {
            if (!(inputProductId is Guid productId))
                return null;

            switch (productId.ToString().ToUpper())
            {
                case "D92A7539-C2CD-42D8-BBBC-BEB445D2C162":
                    return new IndusoftProduct
                    {
                        Id = new Guid("D92A7539-C2CD-42D8-BBBC-BEB445D2C162"),
                        Name = "I-DS/P",
                        FileName = "InduSoft.DSP.Server.lic"
                    };
                case "585A89CC-FC9F-4F7E-8642-51BC58FBC64C":
                    return new IndusoftProduct
                    {
                        Id = new Guid("585A89CC-FC9F-4F7E-8642-51BC58FBC64C"),
                        Name = "I-DS/TSDB",
                        FileName = "InduSoft.DSTSDB.Server.lic"
                    };
                case "0E2BC447-ECCD-4EA1-99B5-46A6B5A75FFA":
                    return new IndusoftProduct
                    {
                        Id = new Guid("0E2BC447-ECCD-4EA1-99B5-46A6B5A75FFA"),
                        Name = "I-DS/CENG",
                        FileName = "InduSoft.DSCENG.Server.lic"
                    };
                case "61D2BE1E-A3A8-4DE6-9BEA-047B7EF9366D":
                    return new IndusoftProduct
                    {
                        Id = new Guid("61D2BE1E-A3A8-4DE6-9BEA-047B7EF9366D"),
                        Name = "I-DS/LDS",
                        FileName = "InduSoft.DSLDS.lic"
                    };
                case "A859718B-47F0-4A62-A6FF-34DB2651A4E6":
                    return new IndusoftProduct
                    {
                        Id = new Guid("A859718B-47F0-4A62-A6FF-34DB2651A4E6"),
                        Name = "I-DS/LDS-ECM",
                        FileName = "InduSoft.DSLDSECM.lic"
                    };
                case "C9ACBA59-A6B5-416B-A435-C667B640C534":
                    return new IndusoftProduct
                    {
                        Id = new Guid("C9ACBA59-A6B5-416B-A435-C667B640C534"),
                        Name = "I-DS/LDS-DIM",
                        FileName = "InduSoft.DSLDSDIM.lic"
                    };
                case "C69078D7-6AA9-4591-9CD2-156CD6EC71CB":
                    return new IndusoftProduct
                    {
                        Id = new Guid("C69078D7-6AA9-4591-9CD2-156CD6EC71CB"),
                        Name = "I-DS/RS",
                        FileName = "InduSoft.DSRS.Server.lic"
                    };
                case "E132A26E-F408-45C6-BC0F-F19C00FD24A6":
                    return new IndusoftProduct
                    {
                        Id = new Guid("E132A26E-F408-45C6-BC0F-F19C00FD24A6"),
                        Name = "I-DS/PC-MLS",
                        FileName = "InduSoft.DSPCMLS.lic"
                    };
                case "ED0213DA-E698-40D5-ABA4-3E64D3D22270":
                    return new IndusoftProduct
                    {
                        Id = new Guid("ED0213DA-E698-40D5-ABA4-3E64D3D22270"),
                        Name = "I-DS/PS",
                        FileName = "InduSoft.DSPS.lic"
                    };
                case "D4DEFF77-21FC-4ED4-A38B-7DC55A7FC23E":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("D4DEFF77-21FC-4ED4-A38B-7DC55A7FC23E"),
                        Name = "I-DS/UI",
                        FileName = "InduSoft.DSUI.lic"
                    };
                case "7C2BB9FE-B62E-4398-B4DE-99E5CF3AD393":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("7C2BB9FE-B62E-4398-B4DE-99E5CF3AD393"),
                        Name = "I-DS/PC-EML",
                        FileName = "InduSoft.DSPCEML.lic"
                    };
                case "D902974C-D80C-4802-81EE-1D1300E75EA7":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("D902974C-D80C-4802-81EE-1D1300E75EA7"),
                        Name = "I-DS/PC-RO",
                        FileName = "InduSoft.DSPCRO.lic"
                    };
                case "307DCFDD-E8CF-429C-8B9F-7108DF895F15":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("307DCFDD-E8CF-429C-8B9F-7108DF895F15"),
                        Name = "I-DS/PC-MS",
                        FileName = "InduSoft.DSPCMS.lic"
                    };
                default:
                    return null;
            }
        }

        internal static IndusoftProduct GetIndusoftProductByAssemblyName()
        {
            var entryPointAssamblyName = System.Reflection.Assembly
                .GetEntryAssembly()
                ?.GetName()
                ?.Name;
            switch (entryPointAssamblyName)
            {
                case "IDSP.WebApi":
                    return new IndusoftProduct
                    {
                        Id = new Guid("D92A7539-C2CD-42D8-BBBC-BEB445D2C162"),
                        Name = "I-DS/P",
                        FileName = "InduSoft.DSP.Server.lic"
                    };
                case "TSDB.WebApi":
                    return new IndusoftProduct
                    {
                        Id = new Guid("585A89CC-FC9F-4F7E-8642-51BC58FBC64C"),
                        Name = "I-DS/TSDB",
                        FileName = "InduSoft.DSTSDB.Server.lic"
                    };
                case "CalcRunWebApi":
                    return new IndusoftProduct
                    {
                        Id = new Guid("0E2BC447-ECCD-4EA1-99B5-46A6B5A75FFA"),
                        Name = "I-DS/CENG",
                        FileName = "InduSoft.DSCENG.Server.lic"
                    };
                case "Goober.LDS.DPM.WebApi":
                    return new IndusoftProduct
                    {
                        Id = new Guid("61D2BE1E-A3A8-4DE6-9BEA-047B7EF9366D"),
                        Name = "I-DS/LDS",
                        FileName = "InduSoft.DSLDS.lic"
                    };
                case "Goober.LDS.ECM.WebApp":
                    return new IndusoftProduct
                    {
                        Id = new Guid("A859718B-47F0-4A62-A6FF-34DB2651A4E6"),
                        Name = "I-DS/LDS-ECM",
                        FileName = "InduSoft.DSLDSECM.lic"
                    };
                case "Goober.LDS.DIM.WebApi":
                    return new IndusoftProduct
                    {
                        Id = new Guid("C9ACBA59-A6B5-416B-A435-C667B640C534"),
                        Name = "I-DS/LDS-DIM",
                        FileName = "InduSoft.DSLDSDIM.lic"
                    };
                case "IDSRS.WebApp":
                    return new IndusoftProduct
                    {
                        Id = new Guid("C69078D7-6AA9-4591-9CD2-156CD6EC71CB"),
                        Name = "I-DS/RS",
                        FileName = "InduSoft.DSRS.Server.lic"
                    };
                case "IDSMLS.WebApp":
                    return new IndusoftProduct
                    {
                        Id = new Guid("E132A26E-F408-45C6-BC0F-F19C00FD24A6"),
                        Name = "I-DS/PC-MLS",
                        FileName = "InduSoft.DSPCMLS.lic"
                    };
                case "IDS.Web":
                    return new IndusoftProduct
                    {
                        Id = new Guid("D902974C-D80C-4802-81EE-1D1300E75EA7"),
                        Name = "I-DS/PC-RO",
                        FileName = "InduSoft.DSPCRO.lic"
                    };
                case "Goober.PS.DataAccess.Service":
                case "Goober.PS.Api.Service":
                case "Goober.PS.Schema.Service":
                case "Goober.PS.Dispatcher.Service":
                case "Goober.PS.DataExcelConnector.Service":
                case "Goober.PS.SchemaExcelConnector.Service":
                case "Goober.PS.SchemaIdspConnector.Service":
                case "Goober.PS.ShipmentMath.Service":
                case "Goober.PS.EventLog.Service":
                    return new IndusoftProduct
                    {
                        Id = new Guid("ED0213DA-E698-40D5-ABA4-3E64D3D22270"),
                        Name = "I-DS/PS",
                        FileName = "InduSoft.DSPS.lic"
                    };
                case "IDS.UI.WebApp":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("D4DEFF77-21FC-4ED4-A38B-7DC55A7FC23E"),
                        Name = "I-DS/UI",
                        FileName = "InduSoft.DSUI.lic"
                    };
                case "EML.WebApi":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("7C2BB9FE-B62E-4398-B4DE-99E5CF3AD393"),
                        Name = "I-DS/PC-EML",
                        FileName = "InduSoft.DSPCEML.lic"
                    };
                case "IOMS.WebApi":
                case "IOMS.WorkerService":
                case "I-DS-OMS-NetCore":
                    return new IndusoftProduct()
                    {
                        Id = new Guid("307DCFDD-E8CF-429C-8B9F-7108DF895F15"),
                        Name = "I-DS/PC-MS",
                        FileName = "InduSoft.DSPCMS.lic"
                    };
                default:
                    return null;
            }
        }

        internal static string GetValidationMessageBy(LicenseValidationResult result)
        {
            string message;

            switch (result)
            {
                case LicenseValidationResult.Success:
                    message = null;
                    break;
                case LicenseValidationResult.NoFoundLicenseFile:
                    message = "Не найден лицензионный файл";
                    break;
                case LicenseValidationResult.NoParsedLicense:
                    message = "Лицензионный файл имеет поврежденную структуру";
                    break;
                case LicenseValidationResult.NoCorrectSignature:
                    message = "Лицензионный файл не прошел верификацию";
                    break;
                case LicenseValidationResult.NoDeserializedLicense:
                    message = "Ошибка десириализации лицензионного файла";
                    break;
                case LicenseValidationResult.NoCorrectEnvironmentSignature:
                    message = "Лицензионный файл не соответствует сигнатуре окружения";
                    break;
                case LicenseValidationResult.NoValidProduct:
                    message = "Лицензионный файл не соответствует запущенному приложению";
                    break;
                case LicenseValidationResult.NoValidDate:
                    message = "Срок действия лицензии истек";
                    break;
                default:
                case LicenseValidationResult.Unknown:
                    message = string.Empty;
                    break;
            }

            return message;
        }
    }
}
