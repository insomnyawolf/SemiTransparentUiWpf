using ConfigHelper;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace SemitransparentUi
{
    public class Settings
    {
        public static ConfigurationHelper<Settings> ConfigurationHelper { get; } = new ConfigurationHelper<Settings>("./settings.json", null);
        public static Settings Config { get => ConfigurationHelper.Config; }

        public Theme Theme { get; set; } = new Theme();

        public WindowSettings WindowSettings { get; set; } = new WindowSettings();

        public string SourceUrl { get; set; } = "https://rubend447.fluctis.com/Grupos_Musica/";
    }

    public class WindowSettings
    {
        public int Width { get; set; } = 300;
        public int Height { get; set; } = 500;
    }

    public class Theme
    {
        public Color BackgroundColor { get; set; } = new Color { A = 150, R = 0, G = 0, B = 0 };
        public Color BackgroundAccent { get; set; } = new Color { A = 50, R = 100, G = 100, B = 100 };
        public Color Accent { get; set; } = new Color { A = 255, R = 192, G = 192, B = 192 };
        public Color Secondary { get; set; } = new Color { A = 255, R = 192, G = 192, B = 192 };
        public Color Transparent { get; set; } = new Color { A = 0, R = 0, G = 0, B = 0 };
    }

    public class Color
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        [JsonIgnore]
        public SolidColorBrush Brush { get => new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B)); }

    }
}
