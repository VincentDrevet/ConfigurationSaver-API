using AutoMapper;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dto;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
    }
}