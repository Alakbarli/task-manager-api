using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.DTO.HelperModels;

namespace TaskManagerApi.DTO.ResponseModels
{
    public class TaskVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public List<UserShortInfo> Users { get; set; }
    }
}
