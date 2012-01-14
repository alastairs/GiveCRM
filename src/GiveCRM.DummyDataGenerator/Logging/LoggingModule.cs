using System;
using Ninject.Extensions.Logging;
using Ninject.Modules;

namespace GiveCRM.DummyDataGenerator.Logging
{
    internal class LoggingModule : NinjectModule
    {
        private readonly Action<string> logAction;

        public LoggingModule(Action<string> logAction)
        {
            this.logAction = logAction;
        }

        public override void Load()
        {
            this.Kernel.Bind<Action<string>>().ToMethod(_ => this.logAction);
            this.Kernel.Bind<ILogger>().To<WpfControlLogger>();
        }
    }
}
