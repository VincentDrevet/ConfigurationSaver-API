using Dto;
using Models;

namespace Interfaces
{
    public interface IScheduleTaskRepository
    {
        public ICollection<ScheduleTask> GetAllScheduleTask();
        public ScheduleTask GetScheduleTaskById(Guid id);
        public bool IsScheduleTaskExist(Guid id);
        public ICollection<Server> GetServersInTask(Guid id);
        public ScheduleTask CreateScheduleTask(ScheduleTask createScheduleTask);
    }
}