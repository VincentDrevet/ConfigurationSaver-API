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
            return _context.ScheduleTasks.Where(st => st.Id == id).First();
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
    }
}