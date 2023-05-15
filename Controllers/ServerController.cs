using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using ConfigurationSaver_API.Dto;
using Models;

namespace Controllers
{
    [ApiController, Route("api/v1/server")]
    public class ServerController : Controller
    {
        private readonly IServerRepository _serverRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IMapper _mapper;

        public ServerController(IServerRepository serverRepository, IMapper mapper, ICredentialRepository credentialRepository)
        {
            _serverRepository = serverRepository;
            _credentialRepository = credentialRepository;
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

        [HttpGet, Route("{id}/credential")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetCredentialByServerId(Guid id) {

            if(!_serverRepository.IsServerExist(id))
            {
                return NotFound();
            }

            var credential = _serverRepository.GetCredentialByServerId(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<CredentialDto>(credential));
        }

        [HttpPost, Route("")]
        [ProducesResponseType(typeof(ServerDto), 200)]
        [ProducesResponseType(500)]
        public IActionResult CreateServer([FromQuery] Guid credentialId, CreateServerDto createServer)
        {
            if(createServer == null)
            {
                return BadRequest(ModelState);
            }

            if(!_credentialRepository.IsCredentialExist(credentialId))
            {
                ModelState.AddModelError("", "Credential does not exist.");
                return BadRequest(ModelState);
            }

            var credential = _credentialRepository.GetCredentialById(credentialId);

            var serverMap = _mapper.Map<Server>(createServer);

            serverMap.Credential = credential;

            try
            {
                var createdServer = _serverRepository.CreateServer(serverMap);
                return Ok(_mapper.Map<ServerDto>(createdServer));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}