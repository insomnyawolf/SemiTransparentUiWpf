using CrashHelper;
using System.Windows;

namespace SemiTransparentUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.AddCrashHelper("https://github.com/insomnyawolf/SemiTransparentUiWpf");
        }
    }
}
