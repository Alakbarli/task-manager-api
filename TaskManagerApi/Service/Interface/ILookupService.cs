using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;

namespace TaskManagerApi.Service.Interface
{
    public interface ILookupService
    {
        List<Status> GetStatuses();
    }
}
