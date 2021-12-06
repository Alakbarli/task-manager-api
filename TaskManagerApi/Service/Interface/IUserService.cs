using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.DTO.ResponseModels;
using TaskManagerApi.DTO.RequestModels;
using TaskManagerApi.DTO.HelperModels;
using TaskManagerApi.Domain.Models;

namespace TaskManagerApi.Service.Interface
{
    public interface IUserService
    {
        Task<AuthResult> AddUser(OperateUserDTO dto);
        Task<AuthResult> EditUser(OperateUserDTO dto);
        Task<AuthResult> DeleteUser(OperateUserDTO dto);
        List<UserVM>GetUsers(int orgId,string userId);
        User UserInfo(string id);
        Task<AuthResult> EditAccount(UserOrganization dto, string userid, bool isAdmin);
    }
}
