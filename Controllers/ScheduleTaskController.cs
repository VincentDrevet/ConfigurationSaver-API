using Microsoft.AspNetCore.Mvc;
using Dto;
using AutoMapper;
using Interfaces;
using ConfigurationSaver_API.Dto;
using Models;
using Hangfire;
using ConfigurationSaver_API.Utils;
using Hangfire.Storage;

namespace Controllers
{
    [ApiController, Route("api/v1/scheduletask")]
    public class ScheduleTaskController : Controller
    {
        private readonly IScheduleTaskRepository _scheduleTaskRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IMapper _mapper;
        private readonly ScheduleTaskHelper _scheduleTaskHelper;

        public ScheduleTaskController(IScheduleTaskRepository scheduleTaskRepository, IMapper mapper,
            IDeviceRepository deviceRepository)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
            _scheduleTaskHelper = new ScheduleTaskHelper(scheduleTaskRepository);
        }

        /// <summary>
        /// Return all schedule tasks registered
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(ScheduleTaskDto), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetAllScheduleTasks() {

            var tasks = _scheduleTaskRepository.GetAllScheduleTask();

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<ScheduleTaskDto>>(tasks));

        }

        /// <summary>
        /// Return a schedule task by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(ScheduleTaskDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetScheduleTaskById(Guid id) {
            if(!_scheduleTaskRepository.IsScheduleTaskExist(id)) {
                return NotFound();
            }

            var task = _scheduleTaskRepository.GetScheduleTaskById(id);

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ScheduleTaskDto>(task));

        }

        /// <summary>
        /// Return a list of Devices which are attach to the schedule task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}/device")]
        [ProducesResponseType(typeof(ICollection<DeviceDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetDevicesManagedByTaskId(Guid id)
        {
            if(!_scheduleTaskRepository.IsScheduleTaskExist(id))
            {
                return NotFound();
            }

            var Devices = _scheduleTaskRepository.GetDevicesInTask(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<DeviceDto>>(Devices));

        }

        /// <summary>
        /// Create a new schedule task
        /// </summary>
        /// <param name="createScheduleTaskDto"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(ScheduleTaskDto), 201)]
        [ProducesResponseType(500)]
        public IActionResult CreateScheduleTask(CreateScheduleTaskDto createScheduleTaskDto)
        {
            if(createScheduleTaskDto == null)
            {
                return BadRequest(ModelState);
            }

            var scheduleTaskMap = _mapper.Map<ScheduleTask>(createScheduleTaskDto);

            try
            {
                var createdScheduleTask = _scheduleTaskRepository.CreateScheduleTask(scheduleTaskMap);

                RecurringJob.AddOrUpdate(scheduleTaskMap.Id.ToString(), () => _scheduleTaskHelper.RunScheduleTask(createdScheduleTask.Id), Cron.Minutely);

                return StatusCode(201, _mapper.Map<ScheduleTaskDto>(createdScheduleTask));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing schedule task
        /// </summary>
        /// <param name="scheduleTaskId"></param>
        /// <param name="updateScheduleTask"></param>
        /// <returns></returns>
        [HttpPut, Route("")]
        [ProducesResponseType(typeof(ScheduleTaskDto), 202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateScheduleTask(Guid scheduleTaskId, UpdateScheduleTask updateScheduleTask)
        {
            if(updateScheduleTask == null)
            {
                return BadRequest(ModelState);
            }

            if(scheduleTaskId != updateScheduleTask.Id)
            {
                ModelState.AddModelError("", "Id mismatch");
                return BadRequest(ModelState);
            }

            if(!_scheduleTaskRepository.IsScheduleTaskExist(scheduleTaskId))
            {
                return NotFound();
            }

            var scheduleTaskMap = _mapper.Map<ScheduleTask>(updateScheduleTask);

            try
            {
                var updatedScheduleTask = _scheduleTaskRepository.UpdateScheduleTask(scheduleTaskMap);
                return StatusCode(202, _mapper.Map<ScheduleTaskDto>(updateScheduleTask));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an existing schedule task
        /// </summary>
        /// <param name="scheduleTaskId"></param>
        /// <returns></returns>
        [HttpDelete, Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteScheduleTask([FromQuery] Guid scheduleTaskId)
        {
            if(!_scheduleTaskRepository.IsScheduleTaskExist(scheduleTaskId))
            {
                return NotFound();
            }

            try
            {
                _scheduleTaskRepository.DeleteScheduleTask(_scheduleTaskRepository.GetScheduleTaskById(scheduleTaskId));
                return StatusCode(204);
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        /// <summary>
        /// Attach a device to a schedule task
        /// </summary>
        /// <param name="scheduleTaskId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPost, Route("{scheduleTaskId}/device/{deviceId}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult AddDeviceToTask(Guid scheduleTaskId, Guid deviceId)
        {
            if(_deviceRepository.IsDeviceExist(deviceId) == false || _scheduleTaskRepository.IsScheduleTaskExist(scheduleTaskId) == false) {
                return NotFound();
            }

            var device = _deviceRepository.GetDeviceById(deviceId);
            var scheduleTask = _scheduleTaskRepository.GetScheduleTaskById(scheduleTaskId);

            try
            {
                _scheduleTaskRepository.AddDeviceToScheduleTask(scheduleTask, device);
                return StatusCode(202);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove a device from a schedule task
        /// </summary>
        /// <param name="scheduleTaskId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{scheduleTaskId}/device/{deviceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult RemoveDeviceFromScheduleTask(Guid scheduleTaskId, Guid deviceId)
        {
            if (_deviceRepository.IsDeviceExist(deviceId) == false || _scheduleTaskRepository.IsScheduleTaskExist(scheduleTaskId) == false)
            {
                return NotFound();
            }

            var device = _deviceRepository.GetDeviceById(deviceId);
            var scheduleTask = _scheduleTaskRepository.GetScheduleTaskById(scheduleTaskId);

            try
            {
                _scheduleTaskRepository.RemoveDeviceFromScheduleTask(scheduleTask, device);
                return StatusCode(204);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Start a backup schedule task immediately
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("{id}/run")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult RunScheduleTask(Guid id)
        {
            if(!_scheduleTaskRepository.IsScheduleTaskExist(id))
            {
                return NotFound();
            }

            try
            {
                _scheduleTaskHelper.RunScheduleTask(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}/status")]
        [ProducesResponseType(typeof(JobStatusDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetStatusScheduleTask(Guid id)
        {
            if (!_scheduleTaskRepository.IsScheduleTaskExist(id))
            {
                return NotFound();
            }

            try
            {
                var jobs = JobStorage.Current.GetConnection().GetRecurringJobs();
                var job = jobs.Where(j => j.Id == id.ToString()).FirstOrDefault();

                return Ok(new JobStatusDto
                {
                    CreatedAt = job.CreatedAt,
                    LastRunTime = job.LastExecution,
                    LastJobState = job.LastJobState,
                    NextRunTime = job.NextExecution
                });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}