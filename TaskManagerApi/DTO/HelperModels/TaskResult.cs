using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.DTO.HelperModels
{
    public class TaskResult
    {
        public Domain.Models.Task Task { get; set; }
        public List<string> Errors { get; set; }
        public int Code { get; set; }
    }
}
