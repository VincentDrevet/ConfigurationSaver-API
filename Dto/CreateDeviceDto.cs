using ConfigurationSaver_API.Models;

namespace ConfigurationSaver_API.Dto
{
    public class CreateDeviceDto
    {
        public String Name { get; set; }
        public String IpAddress { get; set; }
        public DeviceTypeEnum Type { get; set; }
    }
}
