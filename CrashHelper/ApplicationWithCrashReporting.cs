using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrashHelper
{
    public static class CrashHelpers
    {
        public static void AddCrashHelper(this Application Application, string GitUrl)
        {
            Application.Dispatcher.UnhandledException += (object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) =>
            {
                DispatchException(sender, e, GitUrl);
            };
        }

        static void DispatchException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e, string GitUrl)
        {
            e.Handled = true;

            var window = new HelperWindow(sender, e, GitUrl);

            window.Show();
        }
    }

    public class HelperWindow : Window
    {
        private string ExceptionString;
        private string GitUrl;
        public HelperWindow(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e, string GitUrl)
        {
            this.GitUrl = GitUrl;

            ExceptionString = e.Exception.ToString();

            Title = "oops";

            var TextColor = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
            var BackGroundColor = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));
            var TextSize = 16d;

            // Create a button    

            Application.Current.MainWindow = this;

            var Size = new Size(800, 600);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Application.Current.MainWindow.Height = Size.Height;
            Application.Current.MainWindow.Width = Size.Width;

            ResizeMode = ResizeMode.NoResize;
            

            var DockPanel = new DockPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                LastChildFill = true
            };

            var textHelp = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                AcceptsTab = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Text = "Something Failed D:\n" +
                "I tried to catch as much errors as i could but it seems like i let that one go trough...\n" +
                "I prepared a button on the bottom so you can help me fixing it, if it's possible, try to explain what were you doing when it happend\n" +
                "The next part is the stack trace, it can help me find where it happend (but sometimes not why), check it if you are interested\n",
                FontSize = TextSize,
                Background = BackGroundColor,
                Foreground = TextColor,
            };

            DockPanel.SetDock(textHelp, Dock.Top);
            DockPanel.Children.Add(textHelp);

            var reportButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = "To report the error click here",
                MinHeight = 50,
                FontSize = TextSize + 2,
                Foreground = TextColor,
                Background = BackGroundColor,
            };
            reportButton.Click += ReportButton_Click;

            DockPanel.SetDock(reportButton, Dock.Bottom);
            DockPanel.Children.Add(reportButton);

            var text = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment= VerticalAlignment.Stretch,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                AcceptsTab = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Text = ExceptionString,
                FontSize = TextSize,
                Background = BackGroundColor,
                Foreground = TextColor,
            };

            DockPanel.SetDock(text, Dock.Top);
            DockPanel.Children.Add(text);

            this.AddChild(DockPanel);
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            PrepareReport(ExceptionString);
        }

        private void PrepareReport(string stack)
        {
            // Prepare URL.
            const string issueTitle = "UnhandledCrash";
            string issueBody = WebUtility.UrlEncode($"StackTrace\n```\n{stack}\n```");

            OpenUrlInBrowser($"{(GitUrl.EndsWith('/') ? GitUrl : GitUrl+'/')}issues/new?title={issueTitle}&body={issueBody}");
        }

        public static void OpenUrlInBrowser(string url)
        {
            // Navigate to a URL.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    CreateNoWindow = true,
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw new NotImplementedException($"'{nameof(OpenUrlInBrowser)}' is not implemented for '{RuntimeInformation.OSDescription}'");
            }
        }
    }
}
