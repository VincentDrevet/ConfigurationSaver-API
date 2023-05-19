using Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Runtime.CompilerServices;

namespace ConfigurationSaver_API.Utils
{
    public class ScheduleTaskHelper
    {
        private readonly IScheduleTaskRepository _scheduleTaskRepository;
        public ScheduleTaskHelper(IScheduleTaskRepository scheduleTaskRepository)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
        }
        public void RunScheduleTask(Guid scheduleTaskId)
        {
            try
            {
                var task = _scheduleTaskRepository.GetScheduleTaskByIdWithRelationShip(scheduleTaskId);

                foreach(var dst in task.DeviceScheduleTasks)
                {
                    dst.Device.RunBackup();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            
        }
    }
}
