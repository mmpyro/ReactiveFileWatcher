using System;

namespace Core.Events
{
    public class FileCreatedEventArgs :  FileChangedEventArgs
    {
        public FileCreatedEventArgs(string message, string name) : base(message, name)
        {
        }
    }
}
