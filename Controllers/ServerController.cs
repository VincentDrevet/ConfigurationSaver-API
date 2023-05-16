using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using ConfigurationSaver_API.Dto;
using Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        /// <summary>
        /// Return all servers registered
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// return a schedule task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get credential attach to the server
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a new server
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="createServer"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(ServerDto), 201)]
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
                return StatusCode(201, _mapper.Map<ServerDto>(createdServer));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// Update an existing server
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="updateServer"></param>
        /// <returns></returns>
        [HttpPut, Route("")]
        [ProducesResponseType(typeof(ServerDto), 202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateServer([FromQuery] Guid serverId, UpdateServerDto updateServer)
        {
            if(updateServer == null)
            {
                return BadRequest(ModelState);
            }
                
            if (serverId != updateServer.Id)
            {
                ModelState.AddModelError("", "Id mismatch");
                return BadRequest(ModelState);
            }
                
            if(!_serverRepository.IsServerExist(serverId))
            {
                return NotFound();
            }

            var serverMap = _mapper.Map<Server>(updateServer);

            try
            {
                var updatedServer = _serverRepository.UpdateServer(serverMap);
                return StatusCode(202, _mapper.Map<ServerDto>(updatedServer));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an existing server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        [HttpDelete, Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteServer([FromQuery] Guid serverId)
        {
            if (!_serverRepository.IsServerExist(serverId))
            {
                return NotFound();
            }

            try
            {
                _serverRepository.DeleteServer(_serverRepository.GetServerById(serverId));
                return StatusCode(202);
            } catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}