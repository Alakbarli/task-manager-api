using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.DTO.ResponseModels
{
    public class BaseResponse
    {
        public int code { get; set; }
        public List<string> errors { get; set; }
        public object data { get; set; }
    }
}
