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
using StudSys.Models;
using StudSys.Contracts.Requests;


namespace StudSys.Controllers
{
    [ApiController]
    public class ClientDataController : ControllerBase
    {
        private readonly IClientDataService _clientDataService;


        public ClientDataController(IClientDataService clientDataService)
        {
            _clientDataService = clientDataService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.ClientData.ChangeGroup)]
        public async Task<IActionResult> ChangeGroupToUserAsync([FromRoute] string username, [FromBody] ChangeGroupToUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();
            var userRoleFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userNameFromJwt == username || userRoleFromJwt != "admin" )
            {
                return BadRequest("Not allowed");
            }

            var clientResponse =  await _clientDataService.ChangeGroupToUser(username, request.GroupId).ConfigureAwait(false);

            if(clientResponse == null)
            {
                return BadRequest("Something wrong");
            }

            return Ok(clientResponse);
        }

    }
}