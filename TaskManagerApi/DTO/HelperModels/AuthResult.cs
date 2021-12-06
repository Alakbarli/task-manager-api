using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;

namespace TaskManagerApi.DTO.HelperModels
{
    public class AuthResult
    {
        public User User { get; set; }
        public List<string> Errors { get; set; }
        public int Code { get; set; }
    }
}
