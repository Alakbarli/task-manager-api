using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.ResponseModels;
using TaskManagerApi.DTO.RequestModels;
using TaskManagerApi.DTO.HelperModels;

namespace TaskManagerApi.Service.Interface
{
    public interface IAuthService
    {
        Task<AuthResult> signUp(RegisterDTO dto);
        Task<AuthResult> Login(LoginDTO dto);
    }
}
