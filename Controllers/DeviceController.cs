using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using ConfigurationSaver_API.Dto;
using Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ConfigurationSaver_API.Models;

namespace Controllers
{
    [ApiController, Route("api/v1/device")]
    public class DeviceController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IMapper _mapper;

        public DeviceController(IDeviceRepository deviceRepository, IMapper mapper, ICredentialRepository credentialRepository)
        {
            _deviceRepository = deviceRepository;
            _credentialRepository = credentialRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all devices registered
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(ICollection<DeviceDto>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetAllDevices() {
            var devices = _deviceRepository.GetAllDevices();

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<ICollection<DeviceDto>>(devices));
        }

        /// <summary>
        /// return a schedule task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(DeviceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetDeviceById(Guid id) {

            if(!_deviceRepository.IsDeviceExist(id)) {
                return NotFound();
            }

            var device = _deviceRepository.GetDeviceById(id);

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            return Ok(_mapper.Map<DeviceDto>(device));
        }

        /// <summary>
        /// Get credential attach to the device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}/credential")]
        [ProducesResponseType(typeof(CredentialDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetCredentialByDeviceId(Guid id) {

            if(!_deviceRepository.IsDeviceExist(id))
            {
                return NotFound();
            }

            var credential = _deviceRepository.GetCredentialByDeviceId(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<CredentialDto>(credential));
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="createDevice"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(DeviceDto), 201)]
        [ProducesResponseType(500)]
        public IActionResult CreateDevice([FromQuery] Guid credentialId, CreateDeviceDto createDevice)
        {
            if(createDevice == null)
            {
                return BadRequest(ModelState);
            }

            if(!_credentialRepository.IsCredentialExist(credentialId))
            {
                ModelState.AddModelError("", "Credential does not exist.");
                return BadRequest(ModelState);
            }

            var credential = _credentialRepository.GetCredentialById(credentialId);

            switch (createDevice.Type)
            {
                case DeviceTypeEnum.EsxiServer:
                    var deviceMap = _mapper.Map<EsxiServer>(createDevice);
                    deviceMap.Credential = credential;
                    return SaveDevice(deviceMap);
                default:
                    return BadRequest();
                    
            }        
        }
        
        /// <summary>
        /// Update an existing device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="updateDevice"></param>
        /// <returns></returns>
        [HttpPut, Route("")]
        [ProducesResponseType(typeof(DeviceDto), 202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateDevice([FromQuery] Guid deviceId, UpdateDeviceDto updateDevice)
        {
            if(updateDevice == null)
            {
                return BadRequest(ModelState);
            }
                
            if (deviceId != updateDevice.Id)
            {
                ModelState.AddModelError("", "Id mismatch");
                return BadRequest(ModelState);
            }
                
            if(!_deviceRepository.IsDeviceExist(deviceId))
            {
                return NotFound();
            }

            var deviceMap = _mapper.Map<Device>(updateDevice);

            try
            {
                var updatedDevice = _deviceRepository.UpdateDevice(deviceMap);
                return StatusCode(202, _mapper.Map<DeviceDto>(updatedDevice));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an existing device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpDelete, Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteDevice([FromQuery] Guid deviceId)
        {
            if (!_deviceRepository.IsDeviceExist(deviceId))
            {
                return NotFound();
            }

            try
            {
                _deviceRepository.DeleteDevice(_deviceRepository.GetDeviceById(deviceId));
                return StatusCode(202);
            } catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private IActionResult SaveDevice(Device device)
        {
            try
            {
                var createdDevice = _deviceRepository.CreateDevice(device);
                return StatusCode(201, _mapper.Map<DeviceDto>(createdDevice));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}