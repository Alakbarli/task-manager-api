using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.DTO.HelperModels;
using TaskManagerApi.DTO.RequestModels;

namespace TaskManagerApi.Service.Interface
{
    public interface ITaskService
    {
        Task<TaskResult> AddTask(OperateTaskDTO dto);
        Task<TaskResult> EditTask(OperateTaskDTO dto);
        Task<TaskResult> DeleteTask(int id);
        List<Domain.Models.Task> GetTasks(int orgId);
    }
}
