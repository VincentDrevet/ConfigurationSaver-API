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
        private readonly IMapper _mapper;

        public ScheduleTaskController(IScheduleTaskRepository scheduleTaskRepository, IMapper mapper)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _mapper = mapper;
        }

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

            return Ok(_mapper.Map<ServerDto>(servers));

        }

        [HttpPost, Route("")]
        [ProducesResponseType(typeof(ScheduleTaskDto), 200)]
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
                return Ok(_mapper.Map<ScheduleTaskDto>(createdScheduleTask));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
    }
}