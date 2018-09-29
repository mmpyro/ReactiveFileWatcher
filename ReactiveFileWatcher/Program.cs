using Core;
using Core.Dtos;
using Core.Filters;
using Core.IO.Factories;
using Core.Providers;
using Core.WatchDog;
using Ninject;
using System;

namespace ReactiveFileWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var kernel = new StandardKernel())
                {
                    kernel.Bind<IFileSystemProxyFactory>().To<FileSystemProxyFactory>().InSingletonScope();
                    kernel.Bind<IConfigurationProvider>().To<JsonConfigurationProvider>().InTransientScope();
                    kernel.Bind<IMonitorServiceFactory>().To<MonitorServiceFactory>().InSingletonScope();
                    kernel.Bind<IFileSystemWatchDog>().To<FileSystemWatchDog>().InTransientScope();

                    var configurationProvider = kernel.Get<JsonConfigurationProvider>();
                    var settings = configurationProvider.Read<WatcherSettings>("./appsettings.json");
                    var watchDog = kernel.Get<FileSystemWatchDog>();
                    watchDog.IncludeSubDirectories()
                        .IncludeSubDirectories()
                        .Filter(Filters.ExtFilter("js"))
                        .Watch(settings.DirectoryPath)
                        .Subscribe(evt => Console.WriteLine(evt.Message));

                    Console.WriteLine("Press any key to finish ...");
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
