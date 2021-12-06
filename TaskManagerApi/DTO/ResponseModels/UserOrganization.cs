using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.DTO.ResponseModels
{
    public class UserOrganization
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
