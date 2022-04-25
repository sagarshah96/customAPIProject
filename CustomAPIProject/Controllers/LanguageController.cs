using CustomAPIProject.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Controllers
{
    [Route("api/{culture:culture}/[controller]/[action]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> localizer;
        public LanguageController(IStringLocalizer<Resource> localizer)
        {
            this.localizer = localizer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(localizer["Language"].Value);
        }
    }
}
