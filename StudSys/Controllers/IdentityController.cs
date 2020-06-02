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
using ApiRoutes = StudSys.ApiRoutes;
using StudSys.Services.Interfaces;


namespace StudSys.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;


        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        //[AllowAnonymous]

        //[Route(ApiRoutes.Identity.Login)]
        //public ActionResult<string> Post(
        //  ClientLoginRequest loginRequest)
        //{
        //    var claims = new Claim[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, loginRequest.UserName)
        //    };

        //    var token = new JwtSecurityToken(
        //        issuer: JwtOptions.Issuer,
        //        audience: JwtOptions.Audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        //        );

        //    string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwtToken;
        //}

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

    }
}