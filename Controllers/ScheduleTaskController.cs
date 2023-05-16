using Microsoft.AspNetCore.Mvc;
using Dto;
using AutoMapper;
using Interfaces;
using ConfigurationSaver_API.Dto;
using Models;

namespace Controllers
{
    [ApiController, Route("api/v1/scheduletask")]
    public class ScheduleTaskController : Controller
    {
        private readonly IScheduleTaskRepository _scheduleTaskRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public ScheduleTaskController(IScheduleTaskRepository scheduleTaskRepository, IMapper mapper,
            IServerRepository serverRepository)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _serverRepository = serverRepository;
            _mapper = mapper;
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
        /// Return a list of servers which are attach to the schedule task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}/server")]
        [ProducesResponseType(typeof(ICollection<ServerDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetServersManagedByTaskId(Guid id)
        {
            if(!_scheduleTaskRepository.IsScheduleTaskExist(id))
            {
                return NotFound();
            }

            var servers = _scheduleTaskRepository.GetServersInTask(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<ServerDto>>(servers));

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
        /// Attach a server to a schedule task
        /// </summary>
        /// <param name="scheduleTaskId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        [HttpPost, Route("{scheduleTaskId}/server/{serverId}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult AddServerToTask(Guid scheduleTaskId, Guid serverId)
        {
            if(_serverRepository.IsServerExist(serverId) == false || _scheduleTaskRepository.IsScheduleTaskExist(scheduleTaskId) == false) {
                return NotFound();
            }

            var server = _serverRepository.GetServerById(serverId);
            var scheduleTask = _scheduleTaskRepository.GetScheduleTaskById(scheduleTaskId);

            try
            {
                _scheduleTaskRepository.AddServerToScheduleTask(scheduleTask, server);
                return StatusCode(202);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove a server from a schedule task
        /// </summary>
        /// <param name="scheduleTaskId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{scheduleTaskId}/server/{serverId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult RemoveServerFromScheduleTask(Guid scheduleTaskId, Guid serverId)
        {
            if (_serverRepository.IsServerExist(serverId) == false || _scheduleTaskRepository.IsScheduleTaskExist(scheduleTaskId) == false)
            {
                return NotFound();
            }

            var server = _serverRepository.GetServerById(serverId);
            var scheduleTask = _scheduleTaskRepository.GetScheduleTaskById(scheduleTaskId);

            try
            {
                _scheduleTaskRepository.RemoveServerFromScheduleTask(scheduleTask, server);
                return StatusCode(204);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}