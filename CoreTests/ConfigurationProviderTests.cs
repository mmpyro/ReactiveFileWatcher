using Core.Dtos;
using Core.IO;
using Core.IO.Factories;
using Core.Providers;
using Ninject;
using NSubstitute;
using System;
using Xunit;

namespace CoreTests
{
    public class ConfigurationProviderTests : IDisposable
    {
        private readonly IFileSystemProxyFactory _fileSystemProxyFactory;
        private readonly IFileSystemProxy _fileSystemProxy;
        private readonly IKernel _kernel;

        public ConfigurationProviderTests()
        {
            _kernel = new StandardKernel();
            _fileSystemProxy = Substitute.For<IFileSystemProxy>();
            string json = "{\"DirectoryPath\":\"D:\testRx\" }";
            _fileSystemProxy.ReadAllText().Returns(json);
            _fileSystemProxyFactory = Substitute.For<IFileSystemProxyFactory>();
            _fileSystemProxyFactory.Create(Arg.Is<string>(t => t == "./appsettings.json")).Returns(_fileSystemProxy);

            _kernel.Bind<IFileSystemProxyFactory>().ToConstant(_fileSystemProxyFactory).InSingletonScope();
            _kernel.Bind<IConfigurationProvider>().To<JsonConfigurationProvider>();
        }

        public void Dispose()
        {
            _kernel?.Dispose();
        }

        [Fact]
        public void ShouldReadConfigurationFromFileWhenReadWasCalled()
        {
            //Given
            var configurationProvider = _kernel.Get<IConfigurationProvider>();

            //When
            var configuration = configurationProvider.Read<WatcherSettings>("./appsettings.json");

            //Then
            Assert.NotStrictEqual(@"D:\testRx", configuration.DirectoryPath);
        }

    }
}
