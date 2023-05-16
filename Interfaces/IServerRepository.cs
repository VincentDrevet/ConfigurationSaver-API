using Models;

namespace Interfaces
{
    public interface IServerRepository {
        public ICollection<Server> GetAllServers();
        public Server GetServerById(Guid id);
        public bool IsServerExist(Guid id);
        public Credential GetCredentialByServerId(Guid id);
        public Server CreateServer(Server server);
        public Server UpdateServer(Server server);
        public void DeleteServer(Server deleteServer);
    }
}