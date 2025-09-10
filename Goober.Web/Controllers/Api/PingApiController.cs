using Goober.Base.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using Goober.Web.Models;

namespace Goober.Web.Controllers.Api
{
    [ApiController]
    public class PingApiController : ControllerBase
    {
        private readonly IDateTimeService _dateTimeService;

        public PingApiController(IDateTimeService dateTimeService,
            IWebHostEnvironment webHostEnvironment)
        {
            _dateTimeService = dateTimeService;
        }

        [HttpGet]
        [Route("/api-base/ping/get")]
        public GetPingResponse Get()
        {
            return new GetPingResponse {
                CurrentDateTime = _dateTimeService.GetDateTimeNow(),
                MachineName = Environment.MachineName
            };
        }
    }
}
