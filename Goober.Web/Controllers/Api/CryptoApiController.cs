using Goober.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Goober.Base.Extensions;
using Goober.Base.Attributes;
using Goober.Web.Filters;

namespace Goober.Web.Controllers.Api
{
    [ApiController]
    public class CryptoApiController : ControllerBase
    {
        [HttpPost]
        [Route("api-base/crypto/encrypt")]
        [SwaggerHideInDocsAttribute]
        [BasicAuthAttribute]
        public EncryptResponse EncryptString([FromBody] EncryptRequest request)
        {
            request.RequiredArgumentNotNull(nameof(request));
            request.RequiredArgumentNotNull(() => request.Text);

            var res = request.Text.EncryptString(request.Key);

            return new EncryptResponse { EncryptedText = res };
        }
    }
}
