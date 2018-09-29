using Core.Events;
using System;

namespace Core.IO
{
    public interface IMonitorService : IDisposable
    {
        event EventHandler<FileModifiedEventArgs> Modified;

        event EventHandler<FileRenamedEventArgs> Renamed;

        event EventHandler<FileDeletedEventArgs> Deleted;

        event EventHandler<FileCreatedEventArgs> Created;

        bool IncludeSubDirectories { get; set; }
    }
}
