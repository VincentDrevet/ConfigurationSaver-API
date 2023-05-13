using Interfaces;
using Models;
using Data;

namespace Repository
{
    public class ServerRepository : IServerRepository
    {
        private readonly DataContext _context;
        public ServerRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Server> GetAllServers()
        {
            return _context.Servers.OrderBy(s => s.Name).ToList();
        }

        public Server GetServerById(Guid id)
        {
            return _context.Servers.Where(s => s.Id == id).First();
        }

        public bool IsServerExist(Guid id)
        {
            return _context.Servers.Any(s => s.Id == id);
        }
    }
}