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
using StudSys.Contracts.Requests;

namespace StudSys.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpPost(ApiRoutes.Role.CreateRole)]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _roleService.CreateRole(request.Name).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost(ApiRoutes.Role.DeleteRole)]
        public async Task<IActionResult> DeleteResult([FromBody]DeleteRoleRequest request)
        {
            if(request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _roleService.DeleteRole(request.Id).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost(ApiRoutes.Role.GetAllRoles)]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _roleService.GetAllRoles().ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost(ApiRoutes.Role.ChangeUserRole)]
        public async Task<IActionResult> ChangeUserRole([FromBody]ChangeUserRoleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _roleService.ChangeUserRole(request.UserName, request.Role).ConfigureAwait(false);

            return Ok(response);
        }
    }
}