using Models;
using Interfaces;
using Data;
using AutoMapper;
using Dto;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ScheduleTaskRepository : IScheduleTaskRepository
    {
        private readonly DataContext _context;

        public ScheduleTaskRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<ScheduleTask> GetAllScheduleTask()
        {
            return _context.ScheduleTasks.OrderBy(st => st.Name).ToList();
        }

        public ICollection<ScheduleTask> GetAllScheduleTaskWithRelationShip()
        {
            return _context.ScheduleTasks.Include(st => st.DeviceScheduleTasks).ThenInclude(dst => dst.Device).ThenInclude(d => d.Credential).ToList();
        }

        public ScheduleTask GetScheduleTaskById(Guid id)
        {
            return _context.ScheduleTasks.Where(st => st.Id == id).Include(st => st.DeviceScheduleTasks).First();
        }

        public bool IsScheduleTaskExist(Guid id)
        {
            return _context.ScheduleTasks.Any(st => st.Id == id);
        }

        public ICollection<Device> GetDevicesInTask(Guid id)
        {
            return _context.DeviceScheduleTasks.Where(sst => sst.ScheduleTaskId == id).Select(sst => sst.Device).ToList();
        }

        public ScheduleTask CreateScheduleTask(ScheduleTask createScheduleTask)
        {
            _context.ScheduleTasks.Add(createScheduleTask);
            _context.SaveChanges();
            return createScheduleTask;
        }

        public ScheduleTask UpdateScheduleTask(ScheduleTask updateScheduleTask)
        {
            _context.ScheduleTasks.Update(updateScheduleTask);
            _context.SaveChanges();
            return updateScheduleTask;
        }

        public void DeleteScheduleTask(ScheduleTask deleteScheduleTask)
        {
            _context.Remove(deleteScheduleTask);
            _context.SaveChanges();
        }

        public void AddDeviceToScheduleTask(ScheduleTask scheduleTask, Device device)
        {
            scheduleTask.DeviceScheduleTasks.Add(new DeviceScheduleTask
            {
                DeviceId = device.Id,
                ScheduleTaskId = scheduleTask.Id
            });

            _context.Update(scheduleTask);
            _context.SaveChanges();
        }

        public void RemoveDeviceFromScheduleTask(ScheduleTask scheduleTask, Device device)
        {

            var sst = scheduleTask.DeviceScheduleTasks.Where(sst => sst.ScheduleTaskId == scheduleTask.Id && sst.DeviceId == device.Id).First();

            scheduleTask.DeviceScheduleTasks.Remove(sst);

            _context.Update(scheduleTask);
            _context.SaveChanges();
        }

        public ScheduleTask GetScheduleTaskByIdWithRelationShip(Guid id)
        {
            return _context.ScheduleTasks.Where(st => st.Id == id).Include(st => st.DeviceScheduleTasks).ThenInclude(sst => sst.Device).ThenInclude(s => s.Credential).First();
        }
    }
}