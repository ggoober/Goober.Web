using Goober.Core.Services;
using Goober.Web.Glossary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using Goober.Core.Attributes;
using Goober.Web.Models;

namespace Goober.Web.Controllers.Api
{
    [ApiController]
    public class PingApiController : ControllerBase
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PingApiController(IDateTimeService dateTimeService,
            IWebHostEnvironment webHostEnvironment)
        {
            _dateTimeService = dateTimeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("/api/ping/get")]
        [SwaggerHideInDocsAttribute(cookieName: SwaggerGlossary.HideInDocsCookieName, password: SwaggerGlossary.HideInDocsPasswordValue)]
        public GetPingResponse Get([FromQuery]string application, 
            [FromQuery]string environment, 
            [FromQuery]string machine)
        {
            if (string.IsNullOrEmpty(application) == false
                && _webHostEnvironment.ApplicationName != application)
            {
                throw new InvalidOperationException($"ApplicationName = {_webHostEnvironment.ApplicationName} not equals {application}");
            }

            if (string.IsNullOrEmpty(environment) == false
                && _webHostEnvironment.EnvironmentName != environment)
            {
                throw new InvalidOperationException($"Environment = {_webHostEnvironment.EnvironmentName} not equals {environment}");
            }

            if (string.IsNullOrEmpty(machine) == false
                && Environment.MachineName != machine)
            {
                throw new InvalidOperationException($"MachineName = {Environment.MachineName} not equals {machine}");
            }

            return new GetPingResponse {
                ApplicationName = _webHostEnvironment.ApplicationName,
                AssemblyVersion = ProgramUtils.AssemblyVersion,
                CurrentDateTime = _dateTimeService.GetDateTimeNow(),
                Environment = _webHostEnvironment.EnvironmentName,
                MachineName = Environment.MachineName
            };
        }
    }
}
