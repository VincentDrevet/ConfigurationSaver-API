using Interfaces;
using Models;
using Data;

namespace Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DataContext _context;
        public DeviceRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Device> GetAllDevices()
        {
            return _context.Devices.OrderBy(s => s.Name).ToList();
        }

        public Device GetDeviceById(Guid id)
        {
            return _context.Devices.Where(s => s.Id == id).First();
        }

        public bool IsDeviceExist(Guid id)
        {
            return _context.Devices.Any(s => s.Id == id);
        }

        public Credential GetCredentialByDeviceId(Guid id)
        {
            return _context.Devices.Where(s => s.Id == id).Select(s => s.Credential).First();
        }

        public Device CreateDevice(Device device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }

        public Device UpdateDevice(Device device)
        {
            _context.Update(device);
            _context.SaveChanges();
            return device;
        }

        public void DeleteDevice(Device deleteDevice)
        {
            _context.Remove(deleteDevice);
            _context.SaveChanges();
        }
    }
}