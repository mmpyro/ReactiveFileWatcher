using System;

namespace Core.Events
{
    public abstract class FileChangedEventArgs : EventArgs
    {
        public virtual string Message { get; protected set; }
        public virtual string Name { get; protected set; }

        protected FileChangedEventArgs(string message, string name)
        {
            Message = message;
            Name = name;
        }
    }
}