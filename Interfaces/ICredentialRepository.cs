using Models;

namespace Interfaces
{
    public interface ICredentialRepository
    {
        public ICollection<Credential> GetAllCredential();
        public Credential GetCredentialById(Guid id);
        public bool IsCredentialExist(Guid id);
    }
}