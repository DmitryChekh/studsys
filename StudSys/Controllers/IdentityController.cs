using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudSys.Requests;
using StudSys.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using ApiRoutes;
using StudSys.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace StudSys.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;


        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

      

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> RegisterClient([FromBody]ClientRegistrationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            //var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            //if (userRole != "admin")
            //{
            //    return Forbid();
            //}


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }

            var authResponse = await _identityService.RegisterAsync(
                request.Email,
                request.FirstName,
                request.LastName,
                request.MiddleName,
                request.Password,
                "user");


            if (!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

            return Ok(authResponse);
        }





        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody]ClientLoginRequest request)
        {
            if(request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var authResponse = await _identityService.LoginAsync(request.UserName, request.Password);

            if(!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

            return Ok(authResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Identity.CheckJWT)]
        public IActionResult CheckJWT()
        {
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            var response = new { tokenActive = true, userName = userNameFromJwt };

            return Ok(response);
        }

    }
}