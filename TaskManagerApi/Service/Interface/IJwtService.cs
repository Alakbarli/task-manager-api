using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.ResponseModels;

namespace TaskManagerApi.Service.Interface
{
    public interface IJwtService
    {
        JwtResponse GenerateToken(JwtCustomClaim claims);
    }
}
