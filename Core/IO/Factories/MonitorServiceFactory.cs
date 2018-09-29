namespace Core.IO.Factories
{
    public class MonitorServiceFactory : IMonitorServiceFactory
    {
        public IMonitorService Create(string path)
        {
            return new FileMonitorService(path);
        }
    }
}
