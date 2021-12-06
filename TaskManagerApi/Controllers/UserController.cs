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
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UserController(IJwtService jwtService, IUserService userService, IMapper mapper)
        {
            _jwtService = jwtService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost, Route("adduser")]
        public async Task<ActionResult> AddUser(OperateUserDTO dto)
        {
            dto.OrgId = Convert.ToInt32(User.Claims.First(x => x.Type == "OrganizationId").Value);

            AuthResult result = await _userService.AddUser(dto);

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors });
        }

        [HttpPost, Route("edituser")]
        public async Task<ActionResult> EditUser(OperateUserDTO dto)
        {
            AuthResult result = await _userService.EditUser(dto);

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors });
        }

        [HttpPost, Route("deleteuser")]
        public async Task<ActionResult> DeleteUser(OperateUserDTO dto)
        {
            AuthResult result = await _userService.DeleteUser(dto);

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors });
        }

        [HttpGet,Route("getusers")]
        public ActionResult GetUsers()
        {
            int orgId = Convert.ToInt32(User.Claims.First(x => x.Type == "OrganizationId").Value);
            string userId =User.Claims.First(x => x.Type == "id").Value;

            var users = _userService.GetUsers(orgId,userId);

            return Ok(new BaseResponse { code = 0, data = users });

        }

        [HttpGet, Route("getuserinfo")]
        public ActionResult GetUserInfo()
        {
            string userId = User.Claims.First(x => x.Type == "id").Value;

            var user = _userService.UserInfo(userId);

            var userInfo = _mapper.Map<User, UserOrganization>(user);

            return Ok(new BaseResponse { code = 0, data = userInfo });

        }

        [HttpPost, Route("editaccount")]
        public async Task<ActionResult> EditAccount(UserOrganization dto)
        {
            string userId = User.Claims.First(x => x.Type == "id").Value;

            string role = User.Claims.First(x => x.Type == "RoleName").Value;

            bool isAdmin = role == "admin";

            AuthResult result = await _userService.EditAccount(dto, userId, isAdmin);

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
                    RoleName = role,
                    Username = result.User.UserName
                };

                jwtRes = _jwtService.GenerateToken(jwtCustomClaim);
            }

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors, data = jwtRes?.Token });
        }
    }
}
