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

        [HttpGet, Route("")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
        public IActionResult GetCredentials() {

            var credentials = _credentialRepository.GetAllCredential();

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<CredentialDto>>(credentials));
        }

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

        [HttpGet, Route("{id}/server")]
        [ProducesResponseType(typeof(ICollection<ServerDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetServersByCredentialId(Guid id)
        {

            if (!_credentialRepository.IsCredentialExist(id))
            {
                return NotFound();
            }

            var servers = _credentialRepository.GetServersByCredentialId(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<ServerDto>>(servers));
        }

        [HttpPost, Route("")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
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
                return Ok(_mapper.Map<CredentialDto>(createdCredential));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut, Route("")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
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
                return Ok(_mapper.Map<CredentialDto>(updatedCredential));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}