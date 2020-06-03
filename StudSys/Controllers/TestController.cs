using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace StudSys.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Test.Text)]
        public ActionResult<string> Get()
        {
            return "Test string";
        }

    }
}