using Goober.CommonModels;
using Goober.Core.Services;
using Goober.WebApi.Glossary;
using Goober.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;


namespace Goober.WebApi.Controllers.Api
{
    [ApiController]
    class CacheApiController : ControllerBase
    {
        private readonly ICacheProvider _cacheProvider;

        public CacheApiController(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        [HttpGet]
        [Route("/api/cache/get-entries")]
        [SwaggerHideInDocsAttribute(cookieName: SwaggerGlossary.HideInDocsCookieName, password: SwaggerGlossary.HideInDocsPasswordValue)]
        public GetCachedEntriesResponse GetCachedEntries([FromQuery]string password)
        {
            if (password != CacheGlossary.CacheApiPasswordValue)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var res = _cacheProvider.GetCachedEntries();
            var ret = new GetCachedEntriesResponse { 
                CachedEntries = res.Select(x=> new GetCachedEntriesModel 
                    { 
                        CacheKey = x.Key, 
                        ExpirationDateTime = x.Value.ExpirationDateTime,
                        ExpirationTimeInMinutes = x.Value.ExpirationTimeInMinutes,
                        IsEmpty = x.Value.IsEmpty,
                        LastAccessDateTime = x.Value.LastAccessDateTime,
                        LastRefreshDateTime = x.Value.LastRefreshDateTime,
                        NextRefreshDateTime = x.Value.NextRefreshDateTime,
                        RefreshTimeInMinutes = x.Value.RefreshTimeInMinutes,
                        RowCreatedDateTime = x.Value.RowCreatedDateTime
                    }).ToList()
            };

            return ret;
        }

        [HttpPost]
        [Route("/api/cache/remove")]
        [SwaggerHideInDocsAttribute(cookieName: SwaggerGlossary.HideInDocsCookieName, password: SwaggerGlossary.HideInDocsPasswordValue)]
        public void Remove([FromQuery] string cacheKey, [FromQuery] string password)
        {
            if (password != CacheGlossary.CacheApiPasswordValue)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.Unauthorized);
            }

            _cacheProvider.Remove(cacheKey);
        }
    }
}
