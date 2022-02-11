using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemitransparentUi.CustomControls
{
    /// <summary>
    /// Lógica de interacción para WebElement.xaml
    /// </summary>
    public partial class WebElement : UserControl
    {
        public string SourceUrl 
        { 
            get
            {
                return WebElement1.Source.ToString();
            } 
            set
            {
                WebElement1.Source = new Uri(value);
            }
        }

        public WebElement()
        {
            InitializeComponent();
            WebElement1.DefaultBackgroundColor = System.Drawing.Color.Transparent;
        }

        public void Navigate(string url)
        {
            WebElement1.NavigateToString(url);
        }
    }
}
