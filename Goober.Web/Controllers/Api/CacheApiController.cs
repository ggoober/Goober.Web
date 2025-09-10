using Goober.Caching.Services;
using Goober.Base.Attributes;
using Goober.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Goober.Web.Filters;

namespace Goober.Web.Controllers.Api
{
    [ApiController]
    public class CacheApiController : ControllerBase
    {
        private readonly ICacheProvider _cacheProvider;

        public CacheApiController(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        [HttpPost]
        [Route("/api-base/cache/get-entries")]
        [SwaggerHideInDocsAttribute]
        [BasicAuthAttribute]
        public GetCachedEntriesResponse GetCachedEntries([FromBody]GetCachedEntriesRequest request)
        {
            //if (password != CacheGlossary.CacheApiPasswordValue)
            //{
            //    throw new System.Web.Http.HttpResponseException(HttpStatusCode.Unauthorized);
            //}

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
        [Route("/api-base/cache/remove")]
        [SwaggerHideInDocsAttribute]
        [BasicAuthAttribute]
        public void Remove([FromQuery] string cacheKey, [FromQuery] string password)
        {
            //if (password != CacheGlossary.CacheApiPasswordValue)
            //{
            //    throw new System.Web.Http.HttpResponseException(HttpStatusCode.Unauthorized);
            //}

            _cacheProvider.Remove(cacheKey);
        }
    }
}
