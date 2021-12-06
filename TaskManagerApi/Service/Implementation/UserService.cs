using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
using Microsoft.EntityFrameworkCore;

namespace TaskManagerApi.Service.Interface
{
    public class UserService:IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<UserToTask> _userTask;
        private readonly IRepository<Organization> _organizations;

        public UserService(UserManager<User> userManager, IRepository<UserToTask> userTask, IRepository<Organization> organizations)
        {
            _userManager = userManager;
            _userTask = userTask;
            _organizations= organizations;
        }

        public async Task<AuthResult> AddUser(OperateUserDTO dto)
        {
            User newUser = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                UserName = dto.UserName,
                OrganizationId = (int)dto.OrgId
            };
            string password = "Aa@111";
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, password);

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
                return result;
            }

            await _userManager.AddToRoleAsync(newUser, "user");

            result.User = newUser;

            return result;
        }

        public async Task<AuthResult> DeleteUser(OperateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);

            var userTask = _userTask.AllQuery.Where(x => x.UserId == user.Id)?.ToList();

            foreach (var item in userTask)
            {
                _userTask.Remove(item);
            }
            await _userTask.SaveAsync();

            IdentityResult identityResult = await _userManager.DeleteAsync(user);

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
                return result;
            }

            return result;
        }

        public async Task<AuthResult> EditAccount(UserOrganization dto,string userid, bool isAdmin)
        {
            var user = await _userManager.FindByIdAsync(userid);
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;

            AuthResult result = new AuthResult
            {
                Code = 0,
                User = null,
                Errors = null
            };

            if (!String.IsNullOrEmpty(dto.NewPassword))
            {
                IdentityResult passwordResult =await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    result.Code = 1;
                    result.Errors = new List<string>();
                    foreach (var item in passwordResult.Errors)
                    {
                        result.Errors.Add(item.Description);
                    }
                    return result;
                }
            }

            IdentityResult identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                result.Code = 1;
                result.Errors = new List<string>();
                foreach (var item in identityResult.Errors)
                {
                    result.Errors.Add(item.Description);
                }
                return result;
            }

            var org = _organizations.AllQuery.FirstOrDefault(x => x.Id == user.OrganizationId);
            if (isAdmin)
            {
                
                org.Name = dto.OrganizationName;
                org.PhoneNumber = dto.PhoneNumber;
                org.Address = dto.Address;
                _organizations.Update(org);
                await _organizations.SaveAsync();
                
            }
            user.Organization = org;

            result.User = user;

            return result;
        }

        public async Task<AuthResult> EditUser(OperateUserDTO dto)
        {
            var user =await _userManager.FindByIdAsync(dto.Id);
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;

            IdentityResult identityResult = await _userManager.UpdateAsync(user);

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
                return result;
            }


            result.User = user;

            return result;
        }

        public List<UserVM> GetUsers(int orgId,string userId)
        {
          return  _userManager.Users
                .Where(u => u.OrganizationId == orgId&&u.Id!=userId)
                .Select(u=>new UserVM {
                    Id=u.Id,
                    Name=u.Name,
                    Surname=u.Surname,
                    Email=u.Email
                })
                .ToList();
        }

        public User UserInfo(string id)
        {
            var user = _userManager.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id == id);
            return user;
        }
    }
}
