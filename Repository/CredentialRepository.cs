using Data;
using Interfaces;
using Models;

namespace Repository
{
    class CredentialRepository : ICredentialRepository
    {
        private readonly DataContext _context;
        public CredentialRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Credential> GetAllCredential()
        {
            return _context.Credentials.OrderBy(c => c.Name).ToList();
        }

        public Credential GetCredentialById(Guid id)
        {
            return _context.Credentials.Where(c => c.Id == id).First();
        }

        public bool IsCredentialExist(Guid id)
        {
            return _context.Credentials.Any(c => c.Id == id);
        }
    }
}