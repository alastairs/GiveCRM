namespace GiveCRM.DummyDataGenerator
{
    using System;
    using GiveCRM.BusinessLogic;
    using GiveCRM.DummyDataGenerator.Generation;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    public partial class App
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            IKernel ninjectKernel = new StandardKernel(new LoggingModule());
            ninjectKernel.Scan(a =>
            {
                a.FromCallingAssembly();
                a.FromAssembliesMatching("GiveCRM.*.dll");
                a.BindWithDefaultConventions();
                a.BindWith(new RegexBindingGenerator("(I)(?<name>.+)(Repository)"));
                a.BindWith(new GenericBindingGenerator(typeof(IRepository<>)));
                a.InThreadScope();
            });

            new GeneratorWindow(ninjectKernel.Get<IDatabaseGenerator>(), ninjectKernel.Get<IMemberGenerator>(), ninjectKernel.Get<ICampaignGenerator>()).Show();
        }
    }

    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Action<string> logAction = logString => { };
            Kernel.Bind<Action<string>>().ToMethod(_ => logAction);
        }
    }
}
