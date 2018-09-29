namespace Core.Providers
{
    public interface IConfigurationProvider
    {
        T Read<T>(string filePath);
    }
}