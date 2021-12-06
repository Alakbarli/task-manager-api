using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.DTO.ResponseModels
{
    public class JwtCustomClaim
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizatioId{ get; set; }
        public string RoleName { get; set; }
    }
}
