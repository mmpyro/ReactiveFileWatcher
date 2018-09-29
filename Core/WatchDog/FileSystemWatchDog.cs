using Core.Events;
using Core.Filters;
using Core.IO;
using Core.IO.Factories;
using System;

namespace Core.WatchDog
{
    public class FileSystemWatchDog : IFileSystemWatchDog
    {
        private readonly IMonitorServiceFactory _monitorServiceFactory;
        private string _filePath;
        private bool _includeSubDir = false;
        private IFileFilter _filter;

        public FileSystemWatchDog(IMonitorServiceFactory monitorServiceFactory)
        {
            _monitorServiceFactory = monitorServiceFactory;
        }

        public IDisposable Subscribe(IObserver<FileChangedEventArgs> observer)
        {
            IMonitorService monitorService = _monitorServiceFactory.Create(_filePath);
            monitorService.IncludeSubDirectories = _includeSubDir;
            monitorService.Created += (s, evt) =>  Notify(observer, evt);
            monitorService.Deleted += (s, evt) =>  Notify(observer, evt);
            monitorService.Renamed += (s, evt) =>  Notify(observer, evt);
            monitorService.Modified += (s, evt) => Notify(observer, evt);
            return monitorService;
        }

        public FileSystemWatchDog Watch(string filePath)
        {
            _filePath = filePath;
            return this;
        }

        public FileSystemWatchDog IncludeSubDirectories()
        {
            _includeSubDir = true;
            return this;
        }

        public FileSystemWatchDog Filter(IFileFilter filter)
        {
            _filter = filter;
            return this;
        }

        private void Notify(IObserver<FileChangedEventArgs> observer, FileChangedEventArgs evt)
        {
            if (_filter == null)
            {
                observer.OnNext(evt);
            }
            else if (_filter.IsMatch(evt))
            {
                observer.OnNext(evt);
            }
        }
    }
}
