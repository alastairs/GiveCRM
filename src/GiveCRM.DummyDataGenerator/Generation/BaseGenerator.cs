using System;

namespace GiveCRM.DummyDataGenerator.Generation
{
    public abstract class BaseGenerator : IBaseGenerator
    {
        internal abstract string GeneratedItemType{get;}
        protected readonly Action<string> LogAction;
        public abstract void Generate(int numberToGenerate);

        protected BaseGenerator(Action<string> logAction)
        {
            this.LogAction = logAction;
        }

        protected void GenerateMultiple(int numberToGenerate, Action createItemCallback)
        {
            ProgressReporter reporter = new ProgressReporter(numberToGenerate);
            LogAction(string.Format("Generating {0} {1}...", numberToGenerate, GeneratedItemType));

            reporter.ReportProgress(createItemCallback, percent => LogAction(percent + "% complete"));
            LogAction(string.Format("{0} {1} generated successfully", numberToGenerate, GeneratedItemType));
        }
    }
}