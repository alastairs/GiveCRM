using GiveCRM.BusinessLogic;
using GiveCRM.DummyDataGenerator.Generation;
using Ninject;
using Ninject.Extensions.Conventions;

namespace GiveCRM.DummyDataGenerator
{
    using GiveCRM.DummyDataGenerator.Logging;

    public partial class App
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            var uiLogger = new UILogger();
            IKernel ninjectKernel = new StandardKernel(new LoggingModule(uiLogger.Log));
            ninjectKernel.Scan(a =>
                                   {
                                       a.FromCallingAssembly();
                                       a.FromAssembliesMatching("GiveCRM.*.dll");
                                       a.BindWithDefaultConventions();
                                       a.BindWith(new RegexBindingGenerator("(I)(?<name>.+)(Repository)"));
                                       a.BindWith(new GenericBindingGenerator(typeof (IRepository<>)));
                                       a.InSingletonScope();
                                   });

            var databaseGenerator = ninjectKernel.Get<IDatabaseGenerator>();
            var memberGenerator = ninjectKernel.Get<IMemberGenerator>();
            var campaignGenerator = ninjectKernel.Get<ICampaignGenerator>();
            var generatorWindow = new GeneratorWindow(databaseGenerator, memberGenerator, campaignGenerator);
            uiLogger.LogAction = generatorWindow.Log;
            generatorWindow.Show();
        }
    }
}
