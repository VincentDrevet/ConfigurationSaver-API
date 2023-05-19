using Dto;
using Models;

namespace Interfaces
{
    public interface IScheduleTaskRepository
    {
        public ICollection<ScheduleTask> GetAllScheduleTask();
        public ICollection<ScheduleTask> GetAllScheduleTaskWithRelationShip();
        public ScheduleTask GetScheduleTaskById(Guid id);
        public bool IsScheduleTaskExist(Guid id);
        public ICollection<Device> GetDevicesInTask(Guid id);
        public ScheduleTask CreateScheduleTask(ScheduleTask createScheduleTask);
        public ScheduleTask UpdateScheduleTask(ScheduleTask updateScheduleTask);
        public void DeleteScheduleTask(ScheduleTask deleteScheduleTask);
        public void AddDeviceToScheduleTask(ScheduleTask scheduleTask, Device device);
        public void RemoveDeviceFromScheduleTask(ScheduleTask scheduleTask, Device device);
        public ScheduleTask GetScheduleTaskByIdWithRelationShip(Guid id);
    }
}