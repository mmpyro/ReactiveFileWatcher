namespace Core.IO.Factories
{
    public interface IMonitorServiceFactory
    {
        IMonitorService Create(string path);
    }
}
