using Core.Events;
using Core.Filters;
using Core.IO;
using Core.IO.Factories;
using Core.WatchDog;
using NSubstitute;
using System;
using Xunit;
using System.Collections.Generic;
using Ninject;

namespace CoreTests
{
    public class WatchDogTests
    {
        private const string FileCreatedEventMessage = "File index.js was created.";
        private const string FileDeletedEventMessage = "File index.js was deleted.";
        private const string FileRenamedEventMessage = "File index.js was rename to index2.js.";
        private const string FileModifiedEventMessage = "File index.js was modified.";
        private const string FileName = "index.js";
        private const string ObserverDirPath = @"D:\testRx";
        private readonly IMonitorServiceFactory _monitorServiceFactory;
        private readonly IMonitorService _monitorService;
        private readonly IKernel _kernel;

        public WatchDogTests()
        {
            _monitorService = Substitute.For<IMonitorService>();
            _monitorServiceFactory = Substitute.For<IMonitorServiceFactory>();
            _monitorServiceFactory.Create(Arg.Any<string>()).Returns(_monitorService);

            _kernel = new StandardKernel();
            _kernel.Bind<IMonitorServiceFactory>().ToConstant(_monitorServiceFactory).InSingletonScope();
            _kernel.Bind<IFileSystemWatchDog>().To<FileSystemWatchDog>().InTransientScope();
        }

        [Fact]
        public void ShouldNotifyWhenFileWasCreatedInSpecifiedPath()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath).Subscribe(e => createdEvent = e);
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, FileName));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal(FileCreatedEventMessage, createdEvent.Message);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasDeletedInSpecifiedPath()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath).Subscribe(e => createdEvent = e);
            _monitorService.Deleted += Raise.EventWith(new FileDeletedEventArgs(FileDeletedEventMessage, FileName));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal(FileDeletedEventMessage, createdEvent.Message);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasRenamedInSpecifiedPath()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath).Subscribe(e => createdEvent = e);
            _monitorService.Renamed += Raise.EventWith(new FileRenamedEventArgs(FileRenamedEventMessage, "index2.js"));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal(FileRenamedEventMessage, createdEvent.Message);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasModifiedInSpecifiedPath()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath).Subscribe(e => createdEvent = e);
            _monitorService.Modified += Raise.EventWith(new FileModifiedEventArgs(FileModifiedEventMessage, FileName));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal(FileModifiedEventMessage, createdEvent.Message);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasCreatedAndExtIncludedInFilter()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath)
                .Filter(Filters.ExtFilter("js"))
                .Subscribe(e => createdEvent = e);
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, FileName));
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, "index.txt"));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal(FileName, createdEvent.Name);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasCreatedAndExtsIncludedInFilter()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath)
                .Filter(Filters.Complex(Filters.ExtFilter("js"), Filters.ExtFilter("txt")))
                .Subscribe(e => createdEvent = e);
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, FileName));
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, "index.txt"));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal("index.txt", createdEvent.Name);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasCreatedAndNameIncludedInFilter()
        {
            //Given
            var createdEvents = new Stack<FileChangedEventArgs>();
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath)
                .Filter(Filters.Name("index"))
                .Subscribe(e => createdEvents.Push(e));
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, FileName));
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, "index.txt"));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal("index.txt", createdEvents.Pop().Name);
            Assert.Equal(FileName, createdEvents.Pop().Name);
        }

        [Fact]
        public void ShouldNotifyWhenFileWasCreatedAndCustomPredicateMatch()
        {
            //Given
            FileChangedEventArgs createdEvent = null;
            var fileWatchDog = _kernel.Get<IFileSystemWatchDog>();

            //When
            var disp = fileWatchDog.Watch(ObserverDirPath)
                .Filter(Filters.Custom(e => e.Name == "index.html"))
                .Subscribe(e => createdEvent = e);
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, FileName));
            _monitorService.Created += Raise.EventWith(new FileCreatedEventArgs(FileCreatedEventMessage, "index.html"));
            disp.Dispose();

            //Then
            _monitorService.Received(1).Dispose();
            Assert.Equal("index.html", createdEvent.Name);
        }
    }
}
