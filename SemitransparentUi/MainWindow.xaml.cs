using SemitransparentUi;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
//https://github.com/Dirkster99/AvalonEdit-Samples/tree/master/source/99_Edi/TextEditLib/Interfaces
namespace SemiTransparentUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowInteropHelper WindowInteropHelper;
        private readonly Style OverridenButtonStyle;

        public MainWindow()
        {
            OverridenButtonStyle = (Style)FindResource("OverridenButtonStyle");
            WindowInteropHelper = new WindowInteropHelper(this);

            InitializeComponent();

            LoadConfig();
            LoadStyle();

            mainMenuMoveApp.PreviewMouseLeftButtonDown += DragWindow;
            mainMenuExit.Click += MainMenuExitClick;

            // Crash the app
            //Settings.ConfigurationHelper.OnConfigurationChanged += UpdateLayout;
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            //NewMessageBox.
            //ChatHistory.AppendText()
        }


        //private WindowInteropHelper InteropHelper;
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            MouseAbstactions.MoveWindowHelper(WindowInteropHelper.Handle);

            // Not using the following due to weird behaviour
            //DragMove();
        }

        private void LoadConfig()
        {
            Web.SourceUrl = Settings.Config.SourceUrl;
            Height = Settings.Config.WindowSettings.Height;
            Width = Settings.Config.WindowSettings.Width;
        }

        private void LoadStyle()
        {
            this.Background = Settings.Config.Theme.BackgroundColor.Brush;
            this.Foreground = Settings.Config.Theme.Accent.Brush;
            MainMenu.Background = Settings.Config.Theme.BackgroundColor.Brush;
            MainMenu.Foreground = Settings.Config.Theme.Accent.Brush;
        }

        private void ConfigureButton(Button Button)
        {
            Button.FocusVisualStyle = OverridenButtonStyle;
            Button.Style = OverridenButtonStyle;
            Button.OverridesDefaultStyle = true;

            Button.Background = Settings.Config.Theme.BackgroundColor.Brush;
            Button.MouseEnter += (object sender, MouseEventArgs e) => Button.Background = Settings.Config.Theme.BackgroundAccent.Brush;
            Button.GotFocus += (object sender, RoutedEventArgs e) => Button.Background = Settings.Config.Theme.BackgroundAccent.Brush;
            Button.MouseLeave += (object sender, MouseEventArgs e) => Button.Background = Settings.Config.Theme.BackgroundColor.Brush;
            Button.LostFocus += (object sender, RoutedEventArgs e) => Button.Background = Settings.Config.Theme.BackgroundColor.Brush;

            Button.Foreground = Settings.Config.Theme.Accent.Brush;
        }

        private void MainMenuExitClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void CrashTest(object sender, RoutedEventArgs e)
        {
            throw new Exception("WPF HURTS");
        }
    }
}
