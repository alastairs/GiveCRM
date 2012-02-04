using GiveCRM.BusinessLogic;
using Ninject;
using Ninject.Extensions.Conventions;
using GiveCRM.DummyDataGenerator.Logging;
using Ninject.Extensions.Logging;
using System.Windows.Controls;

namespace GiveCRM.DummyDataGenerator
{
    public partial class App
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Scan(a =>
                                   {
                                       a.FromCallingAssembly();
                                       a.FromAssembliesMatching("GiveCRM.*.dll");
                                       a.BindWithDefaultConventions();
                                       a.BindWith(new RegexBindingGenerator("(I)(?<name>.+)(Repository)"));
                                       a.BindWith(new GenericBindingGenerator(typeof (IRepository<>)));
                                       a.InSingletonScope();
                                   });

            ninjectKernel.Bind<ILogger>().To<WpfTextBoxLogger>();
            var generatorWindow = ninjectKernel.Get<IGeneratorWindow>();
            ninjectKernel.Bind<TextBox>().ToConstant(generatorWindow.LogBox);
            generatorWindow.Show();
        }
    }
}
