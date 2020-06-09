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
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Lesson.CreateSubjectLesson)]
        public async Task<IActionResult> CreateSubjectLesson([FromBody]CreateSubjectLessonRequest request)
        {
            if(request == null)
            {
                return BadRequest("Empty request body");
            }

            var result = await _lessonService.CreateSubjectLesson(request.SubjectId, request.GroupId ,request.utxDateTime, request.TypeId).ConfigureAwait(false);

            return Ok(result);
        }

    }
}