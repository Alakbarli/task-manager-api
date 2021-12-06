using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.DTO.ResponseModels;
using TaskManagerApi.Service.Interface;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LookupController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }
        
        [HttpGet,Route("getstatuses")]
        public IActionResult GetStatuses()
        {
            var data=_lookupService.GetStatuses();
            return Ok(new BaseResponse { code = 0, data = data });
        }
    }
}
