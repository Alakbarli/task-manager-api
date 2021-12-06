using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.ResponseModels;
using TaskManagerApi.DTO.RequestModels;
using TaskManagerApi.Infrastructure.Repository;
using TaskManagerApi.Service.Interface;
using TaskManagerApi.DTO.HelperModels;

namespace TaskManagerApi.Service.Interface
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Organization> _organization;

        public AuthService( UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IRepository<Organization> organization)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organization = organization;
        }

        public async Task<AuthResult> Login(LoginDTO dto)
        {
            AuthResult result = new AuthResult
            {
                Code = 0,
                User = null,
                Errors = null
            };

            User user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                result.Code = 1;
                result.Errors=new List<string>() { "Email or password is invalid" };
                return result;
            }
            bool checkPassword =await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!checkPassword)
            {
                result.Code = 1;
                result.Errors = new List<string>() { "Email or password is invalid" };
                return result;
            }
            var roles = await _userManager.GetRolesAsync(user);
            user.Role = roles.FirstOrDefault();
            user.Organization = _organization.AllQuery.FirstOrDefault(x => x.Id == user.OrganizationId);
            result.User = user;
            return result;
        }

        public async Task<AuthResult> signUp(RegisterDTO dto)
        {
            User newUser = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                UserName = dto.UserName
            };
            Organization newOrg = new Organization
            {
                Address = dto.Address,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber
            };

            _organization.Insert(newOrg);
            await _organization.SaveAsync();
            newUser.OrganizationId = newOrg.Id;
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, dto.Password);

            AuthResult result = new AuthResult
            {
                Code = 0,
                User = null,
                Errors = null
            };

            if (!identityResult.Succeeded)
            {
                result.Code = 1;
                result.Errors = new List<string>();
                foreach (var item in identityResult.Errors)
                {
                    result.Errors.Add(item.Description);
                }
                _organization.Remove(newOrg);
                await _organization.SaveAsync();
                return result;
            }

            await _userManager.AddToRoleAsync(newUser, "admin");

            result.User = newUser;

            return result;

        }
    }
}
