using System;
using System.IO;
using Core.Events;

namespace Core.IO
{
    public class FileMonitorService : IMonitorService
    {
        private readonly FileSystemWatcher _fw;

        public FileMonitorService(string filePath)
        {
            _fw = new FileSystemWatcher(filePath);
            _fw.Created += (s, e) => Created(s, new FileCreatedEventArgs($"File {e.Name} was created.", e.Name));
            _fw.Deleted += (s, e) => Deleted(s, new FileDeletedEventArgs($"File {e.Name} was deleted.", e.Name));
            _fw.Renamed += (s, e) => Renamed(s, new FileRenamedEventArgs($"File {e.OldName} was rename to {e.Name}.", e.Name));
            _fw.Changed += (s, e) => Modified(s, new FileModifiedEventArgs($"File {e.Name} was modified.", e.Name));
            _fw.EnableRaisingEvents = true;
        }

        public bool IncludeSubDirectories
        {
            get
            {
                return _fw.IncludeSubdirectories;
            }

            set
            {
                _fw.IncludeSubdirectories = value;
            }
        }

        public event EventHandler<FileModifiedEventArgs> Modified;
        public event EventHandler<FileRenamedEventArgs> Renamed;
        public event EventHandler<FileDeletedEventArgs> Deleted;
        public event EventHandler<FileCreatedEventArgs> Created;

        public void Dispose() {  
            Dispose(true);  
            GC.SuppressFinalize(this);  
        }  

        protected virtual void Dispose(bool disposing) {  
            if (disposing) {  
                _fw?.Dispose();
            }  
        }  
    }
}
