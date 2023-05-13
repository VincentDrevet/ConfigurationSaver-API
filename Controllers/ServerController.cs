using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;

namespace Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class ServerController : Controller
    {
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public ServerController(IServerRepository serverRepository, IMapper mapper)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
        }

        [HttpGet, Route("")]
        [ProducesResponseType(typeof(ICollection<ServerDto>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetAllServers() {
            var servers = _serverRepository.GetAllServers();

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<ServerDto>>(servers));
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(ServerDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetServerById(Guid id) {

            if(!_serverRepository.IsServerExist(id)) {
                return NotFound();
            }

            var server = _serverRepository.GetServerById(id);

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ServerDto>(server));

        }


    }
}