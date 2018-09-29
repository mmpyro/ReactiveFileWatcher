namespace Core.IO.Factories
{
    public interface IFileSystemProxyFactory
    {
        IFileSystemProxy Create(string filePath);
    }
}
