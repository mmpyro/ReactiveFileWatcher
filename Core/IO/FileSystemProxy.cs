using System.IO;

namespace Core.IO
{
    public class FileSystemProxy : IFileSystemProxy
    {
        private readonly string _filePath;

        public FileSystemProxy(string filePath)
        {
            _filePath = filePath;
        }

        public string ReadAllText()
        {
            return File.ReadAllText(_filePath);
        }
    }
}
