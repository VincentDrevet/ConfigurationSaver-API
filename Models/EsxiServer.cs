using Models;

namespace ConfigurationSaver_API.Models
{
    public class EsxiServer : Device
    {
        public override void RunBackup() {

            Console.WriteLine("ESXI");

        }
    }
}