using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.ResponseModels;
using TaskManagerApi.DTO.RequestModels;
using TaskManagerApi.Service.Interface;
using TaskManagerApi.DTO.HelperModels;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _accountService;
        private readonly IUserService _uservice;
        

        public AuthController(IJwtService jwtService, IAuthService accountService)
        {
            _jwtService = jwtService;
            _accountService = accountService;
        }
        [HttpPost, Route("register")]
        public async Task<ActionResult> Register(RegisterDTO dto)
        {
            AuthResult result=await _accountService.signUp(dto);

            JwtResponse jwtRes=null;

            if (result.Code == 0)
            {
                JwtCustomClaim jwtCustomClaim = new JwtCustomClaim
                {
                    Id = result.User.Id,
                    Name = result.User.Name,
                    Surname = result.User.Surname,
                    OrganizationName = result.User.Organization.Name,
                    OrganizatioId = result.User.Organization.Id,
                    RoleName = "admin",
                    Username = result.User.UserName
                };

                jwtRes = _jwtService.GenerateToken(jwtCustomClaim);
            }
            

            return Ok(new {code=result.Code,errors=result.Errors,token=jwtRes?.Token });
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid client request");
            }

            AuthResult result = await _accountService.Login(dto);

            JwtResponse jwtRes = null;

            if (result.Code == 0)
            {
                JwtCustomClaim jwtCustomClaim = new JwtCustomClaim
                {
                    Id = result.User.Id,
                    Name = result.User.Name,
                    Surname = result.User.Surname,
                    OrganizationName = result.User.Organization.Name,
                    OrganizatioId = result.User.Organization.Id,
                    RoleName = result.User.Role,
                    Username = result.User.UserName
                };

                jwtRes = _jwtService.GenerateToken(jwtCustomClaim);
            }


            return Ok(new { code = result.Code, errors = result.Errors, token = jwtRes?.Token });
        }

        
    }
}
