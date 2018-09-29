namespace Core.Events
{
    public class FileModifiedEventArgs : FileChangedEventArgs
    {
        public FileModifiedEventArgs(string message, string name) : base(message, name)
        {
        }
    }
}
