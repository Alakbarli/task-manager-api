using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.HelperModels;
using TaskManagerApi.DTO.RequestModels;
using TaskManagerApi.Infrastructure.Repository;
using TaskManagerApi.Service.Interface;
using Task = TaskManagerApi.Domain.Models.Task;

namespace TaskManagerApi.Service.Interface
{
    public class TaskService : ITaskService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Organization> _organization;
        private readonly IRepository<Task> _task;
        private readonly IRepository<UserToTask> _userTask;

        public TaskService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, 
            IRepository<Organization> organization,
            IRepository<Task> task,
            IRepository<UserToTask> userTask
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organization = organization;
            _task = task;
            _userTask= userTask;
        }

        public async Task<TaskResult> AddTask(OperateTaskDTO dto)
        {
            Task newTask = new()
            {
                Title = dto.Title,
                Description = dto.Description,
                Deadline =Convert.ToDateTime(dto.Deadline),
                OrganizationId = (int)dto.OrganizationId,
                StatusId = dto.StatusId
            };

            _task.Insert(newTask);
            await _task.SaveAsync();

            if (dto.Users != null && dto.Users.Length > 0)
            {
                foreach (var item in dto.Users)
                {
                    UserToTask ut = new()
                    {
                        UserId = item,
                        TaskId = newTask.Id
                    };
                    _userTask.Insert(ut);
                }
                await  _userTask.SaveAsync();
            }

            TaskResult result = new()
            {
                Code = 0,
                Task = newTask
            };
            return result;

        }

        public async Task<TaskResult> DeleteTask(int id)
        {
            var task = _task.AllQuery.FirstOrDefault(x => x.Id == id);

            var usersList = _userTask.AllQuery.Where(x => x.TaskId == task.Id).ToList();

            foreach (var item in usersList)
            {
                _userTask.Remove(item);
            }

            await _userTask.SaveAsync();


            _task.Remove(task);
            await _task.SaveAsync();

            TaskResult result = new()
            {
                Code = 0
            };
            return result;
        }

        public async Task<TaskResult> EditTask(OperateTaskDTO dto)
        {
            var task =_task.AllQuery.FirstOrDefault(x => x.Id == dto.Id);
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Deadline = Convert.ToDateTime(dto.Deadline);
            task.StatusId = dto.StatusId;

            _task.Update(task);
            await _task.SaveAsync();
            var usersList = _userTask.AllQuery.Where(x => x.TaskId == task.Id).ToList();

            foreach (var item in usersList)
            {
                _userTask.Remove(item);
            }
            await _userTask.SaveAsync();
            if (dto.Users != null && dto.Users.Length > 0)
            {
                foreach (var item in dto.Users)
                {
                    UserToTask ut = new()
                    {
                        UserId = item,
                        TaskId = task.Id
                    };
                    _userTask.Insert(ut);
                }
                await _userTask.SaveAsync();
            }

            TaskResult result = new()
            {
                Code = 0,
                Task = task
            };
            return result;
        }

        public List<Domain.Models.Task> GetTasks(int orgId)
        {
            return  _task.AllQuery.Where(x => x.OrganizationId == orgId).Include(x => x.Status).Include(x => x.UserToTasks).ThenInclude(ut => ut.User).ToList();
        }
    }
}
