using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
//https://github.com/Dirkster99/AvalonEdit-Samples/tree/master/source/99_Edi/TextEditLib/Interfaces
namespace SemiTransparentUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SolidColorBrush BackgroundColor { get; set; } = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
        public SolidColorBrush BackgroundAccent { get; set; } = new SolidColorBrush(Color.FromArgb(50, 100, 100, 100));
        public SolidColorBrush Accent { get; set; } = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));
        public SolidColorBrush Secondary { get; set; } = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));
        public SolidColorBrush Transparent { get; set; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        private readonly WindowInteropHelper WindowInteropHelper;
        private readonly Style OverridenButtonStyle;

        public MainWindow()
        {
            OverridenButtonStyle = (Style)FindResource("OverridenButtonStyle");
            WindowInteropHelper = new WindowInteropHelper(this);


            InitializeComponent();

            SetStyle();
            
            mainMenuMoveApp.PreviewMouseLeftButtonDown += DragWindow;
            mainMenuExit.Click += MainMenuExitClick;
            SendButton.Click += Send;
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

        private void SetStyle()
        {
            this.Background = BackgroundColor;
            this.Foreground = Accent;
            MainMenu.Background = BackgroundColor;
            MainMenu.Foreground = Accent;
            ChatHistory.Background = BackgroundColor;
            ChatHistory.Foreground = Accent;
            NewMessage.Background = BackgroundColor;
            NewMessage.Foreground = Accent;

            ConfigureButton(SendButton);
            


            VerticalSplit.Background = BackgroundAccent;
            HorizontalSplit.Background = BackgroundAccent;

        }

        private void ConfigureButton(Button Button)
        {
            Button.FocusVisualStyle = OverridenButtonStyle;
            Button.Style = OverridenButtonStyle;
            Button.OverridesDefaultStyle = true;

            Button.Background = BackgroundColor;
            Button.MouseEnter += (object sender, MouseEventArgs e) => Button.Background = BackgroundAccent;
            Button.GotFocus += (object sender, RoutedEventArgs e) => Button.Background = BackgroundAccent;
            Button.MouseLeave += (object sender, MouseEventArgs e) => Button.Background = BackgroundColor;
            Button.LostFocus += (object sender, RoutedEventArgs e) => Button.Background = BackgroundColor;

            Button.Foreground = Accent;
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
