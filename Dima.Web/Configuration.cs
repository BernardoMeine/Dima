using MudBlazor.Utilities;
using MudBlazor;

namespace Dima.Web;

public static class Configuration
{
    public const string HttpClientName = "dima";

    public static string BackendUrl { get; set; } = string.Empty;

    public static MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = ["Raleway", "sans-serif"]
            }
        },
        Palette = new PaletteLight
        {
            Primary = new MudColor("#131447"),
            PrimaryContrastText = new MudColor("#000000"),
            Secondary = Colors.LightBlue.Darken3,
            Background = Colors.Grey.Lighten4,
            AppbarBackground = new MudColor("#4a57a1"),
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.White,
            DrawerBackground = Colors.LightBlue.Darken4
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.LightBlue.Accent3,
            Secondary = Colors.LightBlue.Darken3,
            AppbarBackground = Colors.LightBlue.Accent3,
            AppbarText = Colors.Shades.Black,
            PrimaryContrastText = new MudColor("#000000")
        }
    };
}
