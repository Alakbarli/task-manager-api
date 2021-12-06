using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.DTO.RequestModels
{
    public class OperateTaskDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public int StatusId { get; set; }
        public string[] Users { get; set; }
        public int? OrganizationId { get; set; }
    }
}
