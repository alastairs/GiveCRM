using GiveCRM.DummyDataGenerator;
using Ninject;
using Ninject.Extensions.Conventions;

namespace DummyDataGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly StandardKernel kernel;

        public App()
        {
            kernel = new StandardKernel();
            kernel.Scan(a =>
            {
                a.FromCallingAssembly();
                a.FromAssembliesMatching("GiveCRM.*.dll");
                a.BindWithDefaultConventions();
                a.InThreadScope();
            });
        }

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new Main(kernel.Get<IGenerator>());
            mainWindow.Show();
        }
    }
}
