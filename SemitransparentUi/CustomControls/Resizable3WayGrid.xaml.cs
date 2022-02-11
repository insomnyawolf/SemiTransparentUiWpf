using System;
using System.Windows;
using System.Windows.Controls;

namespace SemitransparentUi.CustomControls
{
    /// <summary>
    /// Lógica de interacción para Resizable3WayGrid.xaml
    /// </summary>
    public partial class Resizable3WayGrid : UserControl
    {
        public Resizable3WayGrid()
        {
            InitializeComponent();

            SendButton.Click += SendButton_Click;

            ChatHistory.Background = Settings.Config.Theme.BackgroundColor.Brush;
            ChatHistory.Foreground = Settings.Config.Theme.Accent.Brush;
            NewMessage.Background = Settings.Config.Theme.BackgroundColor.Brush;
            NewMessage.Foreground = Settings.Config.Theme.Accent.Brush;

            VerticalSplit.Background = Settings.Config.Theme.BackgroundAccent.Brush;
            HorizontalSplit.Background = Settings.Config.Theme.BackgroundAccent.Brush;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
