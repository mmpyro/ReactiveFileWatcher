namespace Core.Events
{
    public class FileRenamedEventArgs : FileChangedEventArgs
    {
        public FileRenamedEventArgs(string message, string name) : base(message, name)
        {
        }
    }
}
