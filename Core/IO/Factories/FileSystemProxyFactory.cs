namespace Core.IO.Factories
{
    public class FileSystemProxyFactory : IFileSystemProxyFactory
    {
        public IFileSystemProxy Create(string filePath)
        {
            return new FileSystemProxy(filePath);
        }
    }
}
