using Models;

namespace Interfaces
{
    public interface IScheduleTaskRepository
    {
        public ICollection<ScheduleTask> GetAllScheduleTask();
        public ScheduleTask GetScheduleTaskById(Guid id);
        public bool IsScheduleTaskExist(Guid id);
    }
}