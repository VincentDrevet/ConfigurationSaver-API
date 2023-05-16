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
        public ScheduleTask UpdateScheduleTask(ScheduleTask updateScheduleTask);
        public void DeleteScheduleTask(ScheduleTask deleteScheduleTask);
        public void AddServerToScheduleTask(ScheduleTask scheduleTask, Server server);
        public void RemoveServerFromScheduleTask(ScheduleTask scheduleTask, Server server);
    }
}