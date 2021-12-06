using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.Infrastructure.Repository;
using TaskManagerApi.Service.Interface;

namespace TaskManagerApi.Service.Implementation
{
    public class LookupService : ILookupService
    {
        private readonly IRepository<Status> _status;
        public LookupService(IRepository<Status> status)
        {
            _status = status;
        }
        public List<Status> GetStatuses()
        {
            return _status.All.ToList();
        }
    }
}
