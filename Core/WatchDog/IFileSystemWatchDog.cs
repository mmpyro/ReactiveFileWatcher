using Core.Events;
using Core.Filters;
using System;

namespace Core.WatchDog
{
    public interface IFileSystemWatchDog : IObservable<FileChangedEventArgs>
    {
        FileSystemWatchDog Filter(IFileFilter filter);
        FileSystemWatchDog IncludeSubDirectories();
        FileSystemWatchDog Watch(string filePath);
    }
}