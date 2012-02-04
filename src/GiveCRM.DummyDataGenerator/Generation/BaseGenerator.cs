using System;

namespace GiveCRM.DummyDataGenerator.Generation
{
    using Ninject.Extensions.Logging;

    public abstract class BaseGenerator : IBaseGenerator
    {
        internal abstract string GeneratedItemType{get;}
        protected ILogger Logger { get; private set; }
        public abstract void Generate(int numberToGenerate);

        protected BaseGenerator(ILogger logger)
        {
            this.Logger = logger;
        }

        protected void GenerateMultiple(int numberToGenerate, Action createItemCallback)
        {
            ProgressReporter reporter = new ProgressReporter(numberToGenerate);
            Logger.Info("Generating {0} {1}...", numberToGenerate, GeneratedItemType);

            reporter.ReportProgress(createItemCallback, percent => Logger.Info(percent + "% complete"));
            Logger.Info("{0} {1} generated successfully", numberToGenerate, GeneratedItemType);
        }
    }
}