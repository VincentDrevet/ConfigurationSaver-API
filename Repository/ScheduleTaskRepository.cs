using Models;
using Interfaces;
using Data;
using AutoMapper;

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
    }
}