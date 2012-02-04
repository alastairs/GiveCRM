namespace GiveCRM.DummyDataGenerator
{
    using System.Windows.Controls;

    public interface IGeneratorWindow
    {
        void Show();
        TextBox LogBox { get; }
    }
}