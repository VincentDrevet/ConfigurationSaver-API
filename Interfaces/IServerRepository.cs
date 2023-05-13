using Models;

namespace Interfaces
{
    public interface IServerRepository {
        public ICollection<Server> GetAllServers();
        public Server GetServerById(Guid id);
        public bool IsServerExist(Guid id);
    }
}