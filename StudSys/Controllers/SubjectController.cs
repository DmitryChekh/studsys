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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Subject.CreateSubject)]
        public async Task<IActionResult> CreateSubject([FromBody]CreateSubjectRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid("Not allowed");
            }

            var result = await _subjectService.CreateSubject(request.SubjectName).ConfigureAwait(false);

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Subject.LinkGroupToSubject)]
        public async Task<IActionResult> LinkGroupToSubject([FromBody] LinkGroupToSubjectRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var result = await _subjectService.LinkGroupToSubject(request.Groupid, request.Subjectid).ConfigureAwait(false);

            return Ok(result);
        }

    }
}