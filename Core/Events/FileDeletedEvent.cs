namespace Core.Events
{
    public class FileDeletedEventArgs :  FileChangedEventArgs
    {
        public FileDeletedEventArgs(string message, string name) : base(message, name)
        {

        }
    }
}
