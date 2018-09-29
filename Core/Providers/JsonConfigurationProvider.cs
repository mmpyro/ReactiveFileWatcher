using Core.IO.Factories;
using Newtonsoft.Json;

namespace Core.Providers
{
    public class JsonConfigurationProvider : IConfigurationProvider
    {
        private readonly IFileSystemProxyFactory _fileSystemProxyFactory;

        public JsonConfigurationProvider(IFileSystemProxyFactory fileSystemProxyFactory)
        {
            _fileSystemProxyFactory = fileSystemProxyFactory;
        }

        public T Read<T>(string filePath)
        {
            var fileSystemProxy = _fileSystemProxyFactory.Create(filePath);
            string data = fileSystemProxy.ReadAllText();
            var settings = JsonConvert.DeserializeObject<T>(data);
            return settings;
        }
    }
}
