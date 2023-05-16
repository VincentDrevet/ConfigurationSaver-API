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

        public ScheduleTask GetScheduleTaskById(Guid id)
        {
            return _context.ScheduleTasks.Where(st => st.Id == id).Include(st => st.ServerScheduleTasks).First();
        }

        public bool IsScheduleTaskExist(Guid id)
        {
            return _context.ScheduleTasks.Any(st => st.Id == id);
        }

        public ICollection<Server> GetServersInTask(Guid id)
        {
            return _context.ServerScheduleTasks.Where(sst => sst.ScheduleTaskId == id).Select(sst => sst.Server).ToList();
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

        public void AddServerToScheduleTask(ScheduleTask scheduleTask, Server server)
        {
            scheduleTask.ServerScheduleTasks.Add(new ServerScheduleTask
            {
                ServerId = server.Id,
                ScheduleTaskId = scheduleTask.Id
            });

            _context.Update(scheduleTask);
            _context.SaveChanges();
        }

        public void RemoveServerFromScheduleTask(ScheduleTask scheduleTask, Server server)
        {

            var sst = scheduleTask.ServerScheduleTasks.Where(sst => sst.ScheduleTaskId == scheduleTask.Id && sst.ServerId == server.Id).First();

            scheduleTask.ServerScheduleTasks.Remove(sst);

            _context.Update(scheduleTask);
            _context.SaveChanges();
        }
    }
}