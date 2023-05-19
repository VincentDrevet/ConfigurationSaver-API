using AutoMapper;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dto;
using ConfigurationSaver_API.Dto;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/credential")]
    public class CredentialController : Controller
    {
        private readonly ICredentialRepository _credentialRepository;
        private readonly IMapper _mapper;

        public CredentialController(ICredentialRepository credentialRepository, IMapper mapper)
        {
            _credentialRepository = credentialRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all credentials
        /// </summary>
        /// <returns>Return all credentials</returns>
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
        public IActionResult GetCredentials() {

            var credentials = _credentialRepository.GetAllCredential();

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<CredentialDto>>(credentials));
        }

        /// <summary>
        /// Get a credential by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return a credential object</returns>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetCredentialId(Guid id) {

            if(!_credentialRepository.IsCredentialExist(id)) {
                return NotFound();
            }

            var credential = _mapper.Map<CredentialDto>(_credentialRepository.GetCredentialById(id));

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(credential);
        }

        /// <summary>
        /// return a list of servers which use the credential id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}/server")]
        [ProducesResponseType(typeof(ICollection<DeviceDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetServersByCredentialId(Guid id)
        {

            if (!_credentialRepository.IsCredentialExist(id))
            {
                return NotFound();
            }

            var servers = _credentialRepository.GetDevicesByCredentialId(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<DeviceDto>>(servers));
        }

        /// <summary>
        /// Create a new credential
        /// </summary>
        /// <param name="createCredential"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(CredentialDto), 201)]
        [ProducesResponseType(500)]
        public IActionResult CreateCredential(CreateCredentialDto createCredential)
        {
            if(createCredential == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCredential = _credentialRepository.CreateCredential(_mapper.Map<Credential>(createCredential));
                return StatusCode(201, _mapper.Map<CredentialDto>(createdCredential));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Update an existing credential
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="updateCredential"></param>
        /// <returns></returns>
        [HttpPut, Route("")]
        [ProducesResponseType(typeof(CredentialDto), 202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCredential([FromQuery] Guid credentialId,UpdateCredentialDto updateCredential)
        {
            if(updateCredential == null)
            {
                return BadRequest(ModelState);
            }

            if(updateCredential.Id != credentialId)
            {
                ModelState.AddModelError("", "Id mismatch");
                return BadRequest(ModelState);
            }

            if(!_credentialRepository.IsCredentialExist(credentialId))
            {
                return NotFound();
            }

            var credentialMap = _mapper.Map<Credential>(updateCredential);
            try
            {
                var updatedCredential = _credentialRepository.UpdateCredential(credentialMap);
                return StatusCode(202, _mapper.Map<CredentialDto>(updatedCredential));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Delete an existing credential
        /// </summary>
        /// <param name="credentialId"></param>
        /// <returns></returns>
        [HttpDelete, Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCredential([FromQuery] Guid credentialId)
        {
            if (!_credentialRepository.IsCredentialExist(credentialId))
            {
                return NotFound();
            }

            try
            {
                _credentialRepository.DeleteCredential(_credentialRepository.GetCredentialById(credentialId));
                return StatusCode(204);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}