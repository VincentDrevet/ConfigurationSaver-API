using Dto;
using Models;

namespace Interfaces
{
    public interface ICredentialRepository
    {
        public ICollection<Credential> GetAllCredential();
        public Credential GetCredentialById(Guid id);
        public bool IsCredentialExist(Guid id);
        public ICollection<Device> GetDevicesByCredentialId(Guid id);
        public Credential CreateCredential(Credential createCredential);
        public Credential UpdateCredential(Credential updateCredential);
        public void DeleteCredential(Credential deleteCredential);
    }
}