using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.DTO.HelperModels;
using TaskManagerApi.DTO.RequestModels;
using TaskManagerApi.DTO.ResponseModels;
using TaskManagerApi.Service.Interface;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpPost, Route("addtask")]
        public async Task<ActionResult> AddTask([FromBody]OperateTaskDTO dto)
        {
            dto.OrganizationId = Convert.ToInt32(User.Claims.First(x => x.Type == "OrganizationId").Value);

            TaskResult result = await _taskService.AddTask(dto);

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors });
        }

        [HttpPost, Route("edittask")]
        public async Task<ActionResult> EditTask([FromBody] OperateTaskDTO dto)
        {
            TaskResult result = await _taskService.EditTask(dto);

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors });
        }

        [HttpPost, Route("deletetask")]
        public async Task<ActionResult> DeleteTask([FromBody] int id)
        {
            TaskResult result = await _taskService.DeleteTask(id);

            return Ok(new BaseResponse { code = result.Code, errors = result.Errors });
        }

        [HttpGet, Route("gettasks")]
        public ActionResult GetTasks()
        {
            int orgId = Convert.ToInt32(User.Claims.First(x => x.Type == "OrganizationId").Value);

            var tasks = _taskService.GetTasks(orgId);
            var responseData = _mapper.Map<List<Domain.Models.Task>, List<TaskVM>>(tasks);

            return Ok(new BaseResponse { code = 0, data = responseData });

        }
    }
}
