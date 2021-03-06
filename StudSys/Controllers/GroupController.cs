﻿using System;
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
using StudSys.Contracts.Responses;
using Newtonsoft.Json;
using System.IO;

namespace StudSys.Controllers
{
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupDataService _groupDataService;


        public GroupController(IGroupDataService groupDataService)
        {
            _groupDataService = groupDataService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Group.CreateGroup)]
        public async Task<IActionResult> CreateStudentGroup([FromBody]CreateGroupRequest request)
        {

            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var groupResponse = await _groupDataService.CreateGroup(
                request.GroupName,
                request.MonitorUsername,
                request.CourseLeadUsername
                ).ConfigureAwait(false);

            if (!groupResponse.Success)
            {
                return BadRequest(groupResponse.ErrorsMessages);
            }

            return Ok(groupResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "student")]
        [HttpPost(ApiRoutes.Group.MemberList)]
        public async Task<ActionResult> GetMembersOfGroup([FromBody]MembersOfGroupRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var membersListResponse = await _groupDataService.GetAllMembers(request.GroupName).ConfigureAwait(false);

            if(membersListResponse == null)
            {
                return BadRequest("Something wrong");
            }

            return Ok(membersListResponse);
        }


        [HttpPost(ApiRoutes.Group.GetAllSubjectGroup)]
        public async Task<ActionResult> GetAllSubjectOfGroup([FromBody]CollectionOfSubjectGroupRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var subjectGroupList = await _groupDataService.GetAllSubjectGroup(request.GroupId).ConfigureAwait(false);

            if (subjectGroupList == null)
            {
                return BadRequest("Something wrong");
            }

            return Ok(subjectGroupList);
        }


    }
}