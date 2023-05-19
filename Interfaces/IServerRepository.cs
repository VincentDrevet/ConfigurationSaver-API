using Models;

namespace Interfaces
{
    public interface IDeviceRepository {
        public ICollection<Device> GetAllDevices();
        public Device GetDeviceById(Guid id);
        public bool IsDeviceExist(Guid id);
        public Credential GetCredentialByDeviceId(Guid id);
        public Device CreateDevice(Device device);
        public Device UpdateDevice(Device device);
        public void DeleteDevice(Device deleteDevice);
    }
}