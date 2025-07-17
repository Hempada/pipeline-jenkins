using MudBlazor;

namespace WebUi.Theming;

public class Theme : MudTheme
{
    private static readonly string[] DefaultFontFamily = { "ITCAvantGardePro-Md", "Arial", "sans-serif" };
    private static readonly string[] BoldFontFamily = { "ITCAvantGardePro-Bold", "Arial", "sans-serif" };
    private static readonly string DefaultFontSize = ".875rem";

    private static Theme? instance = null;
    public static Theme Instance => instance ??= new Theme();

    public Theme()
    {
        PaletteLight = new PaletteLight()
        {
            /* DEFAULT COLORS */
            Primary = Colors.Blue.Light,
            PrimaryLighten = Colors.Blue.Lighter,
            PrimaryDarken = Colors.Blue.Darker,
            PrimaryContrastText = Colors.Shades.White,

            Secondary = Colors.Fuchsia.Light,
            SecondaryLighten = Colors.Fuchsia.Lighter,
            SecondaryDarken = Colors.Fuchsia.Darker,
            SecondaryContrastText = Colors.Shades.White,

            Tertiary = Colors.Gold.Light,
            TertiaryLighten = Colors.Gold.Lighter,
            TertiaryDarken = Colors.Gold.Darker,
            TertiaryContrastText = Colors.Shades.White,

            Dark = Colors.Gray.Dark,
            DarkLighten = Colors.Gray.Lighter,
            DarkDarken = Colors.Gray.Darker,
            DarkContrastText = Colors.Shades.White,

            Black = Colors.Shades.Black,

            White = Colors.Shades.White,

            GrayDefault = Colors.Gray.Light,
            GrayLight = Colors.Gray.Light,
            GrayLighter = Colors.Gray.Lighter,
            GrayDark = Colors.Gray.Dark,
            GrayDarker = Colors.Gray.Darker,

            /* TEXT */
            TextPrimary = Colors.Gray.Dark,
            TextSecondary = Colors.Gray.Dark,
            TextDisabled = Colors.Gray.Light,

            /* LAYOUT */
            AppbarBackground = Colors.Shades.White,
            AppbarText = Colors.Blue.Light,

            Background = Colors.Shades.White,

            DrawerBackground = Colors.Shades.White,
            DrawerText = Colors.Gray.Dark,
            DrawerIcon = Colors.Gray.Dark,

            Surface = Colors.Shades.White,

            /* ELEMENTS */
            Info = Colors.Blue.Light,
            InfoLighten = Colors.Blue.Lighter,
            InfoDarken = Colors.Blue.Dark,
            InfoContrastText = Colors.Shades.White,

            Success = Colors.Green.Dark,
            SuccessLighten = Colors.Green.Lighter,
            SuccessDarken = Colors.Green.Darker,
            SuccessContrastText = Colors.Shades.White,

            Warning = Colors.Fuchsia.Darker,
            WarningLighten = Colors.Fuchsia.Lighter,
            WarningDarken = Colors.Fuchsia.Dark,
            WarningContrastText = Colors.Shades.White,

            Error = Colors.Red.Dark,
            ErrorLighten = Colors.Red.Lighter,
            ErrorDarken = Colors.Red.Dark,
            ErrorContrastText = Colors.Shades.White,

            OverlayLight = Colors.Gray.Overlay,
            OverlayDark = Colors.Gray.Overlay,

            Divider = Colors.Gray.Dark,
            DividerLight = Colors.Gray.Dark,

            ActionDefault = Colors.Gray.Dark,
            ActionDisabled = Colors.Gray.Light,
            ActionDisabledBackground = Colors.Gray.Light,

            HoverOpacity = 0.15,
        };

        PaletteDark = new PaletteDark()
        {
            /* DEFAULT COLORS */
            Primary = Colors.Blue.Light,
            PrimaryLighten = Colors.Gray.Lighter,
            PrimaryDarken = Colors.Gray.Darker,
            PrimaryContrastText = Colors.Shades.White,

            Secondary = Colors.Fuchsia.Light,
            SecondaryLighten = Colors.Fuchsia.Lighter,
            SecondaryDarken = Colors.Fuchsia.Darker,
            SecondaryContrastText = Colors.Shades.White,

            Tertiary = Colors.Gold.Light,
            TertiaryLighten = Colors.Gold.Lighter,
            TertiaryDarken = Colors.Gold.Darker,
            TertiaryContrastText = Colors.Shades.White,

            Dark = Colors.Gray.Dark,
            DarkLighten = Colors.Gray.Lighter,
            DarkDarken = Colors.Gray.Darker,
            DarkContrastText = Colors.Shades.White,

            Black = Colors.Shades.Black,

            White = Colors.Shades.White,

            GrayDefault = Colors.Gray.Light,
            GrayLight = Colors.Gray.Light,
            GrayLighter = Colors.Gray.Lighter,
            GrayDark = Colors.Gray.Dark,
            GrayDarker = Colors.Blue.Light,

            /* TEXT */
            TextPrimary = Colors.Shades.White,
            TextSecondary = Colors.Shades.White,
            TextDisabled = Colors.Gray.Light,

            /* LAYOUT */
            AppbarBackground = Colors.Gray.Dark,
            AppbarText = Colors.Shades.White,

            Background = Colors.Gray.Dark,

            DrawerBackground = Colors.Gray.Dark,
            DrawerText = Colors.Shades.White,
            DrawerIcon = Colors.Shades.White,

            // Old color user - keept for future references
            //Surface = Colors.Gray.Darker,
            Surface = "#303030",

            /* ELEMENTS */
            Info = Colors.Blue.Light,
            InfoLighten = Colors.Blue.Lighter,
            InfoDarken = Colors.Blue.Dark,
            InfoContrastText = Colors.Shades.White,

            Success = Colors.Green.Darker,
            SuccessLighten = Colors.Green.Lighter,
            SuccessDarken = Colors.Green.Dark,
            SuccessContrastText = Colors.Shades.White,

            Warning = Colors.Fuchsia.Darker,
            WarningLighten = Colors.Fuchsia.Lighter,
            WarningDarken = Colors.Fuchsia.Dark,
            WarningContrastText = Colors.Shades.White,

            Error = Colors.Red.Dark,
            ErrorLighten = Colors.Red.Lighter,
            ErrorDarken = Colors.Red.Dark,
            ErrorContrastText = Colors.Shades.White,

            OverlayLight = Colors.Gray.Overlay,
            OverlayDark = Colors.Gray.Overlay,

            Divider = Colors.Gray.Lighter,
            DividerLight = Colors.Gray.Lighter,

            ActionDefault = Colors.Shades.White,
            ActionDisabled = Colors.Gray.Light,
            ActionDisabledBackground = Colors.Gray.Light,

            HoverOpacity = 0.25,
        };

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "3px",
        };

        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = DefaultFontFamily,
                FontSize = DefaultFontSize,
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = "0"
            },
            H1 = new H1()
            {
                FontFamily = BoldFontFamily,
                FontSize = "6rem",
                FontWeight = 300,
                LineHeight = 1.167,
                LetterSpacing = "0"
            },
            H2 = new H2()
            {
                FontFamily = BoldFontFamily,
                FontSize = "3.75rem",
                FontWeight = 300,
                LineHeight = 1.2,
                LetterSpacing = "0"
            },
            H3 = new H3()
            {
                FontFamily = BoldFontFamily,
                FontSize = "3rem",
                FontWeight = 400,
                LineHeight = 1.167,
                LetterSpacing = "0"
            },
            H4 = new H4()
            {
                FontFamily = BoldFontFamily,
                FontSize = "2.125rem",
                FontWeight = 400,
                LineHeight = 1.235,
                LetterSpacing = "0"
            },
            H5 = new H5()
            {
                FontFamily = BoldFontFamily,
                FontSize = "1.5rem",
                FontWeight = 400,
                LineHeight = 1.334,
                LetterSpacing = "0"
            },
            H6 = new H6()
            {
                FontFamily = BoldFontFamily,
                FontSize = "1.25rem",
                FontWeight = 400,
                LineHeight = 1.6,
                LetterSpacing = "0"
            },
            Button = new Button()
            {
                FontFamily = DefaultFontFamily,
                FontSize = DefaultFontSize,
                FontWeight = 500,
                LineHeight = 1.75,
                LetterSpacing = "0"
            },
            Body1 = new Body1()
            {
                FontFamily = DefaultFontFamily,
                FontSize = "1rem",
                FontWeight = 400,
                LineHeight = 1.5,
                LetterSpacing = "0"
            },
            Body2 = new Body2()
            {
                FontFamily = DefaultFontFamily,
                FontSize = DefaultFontSize,
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = "0"
            },
            Caption = new Caption()
            {
                FontFamily = DefaultFontFamily,
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = "0"
            },
            Subtitle1 = new Subtitle1()
            {
                FontFamily = DefaultFontFamily,
                FontSize = DefaultFontSize,
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = "0"
            },
            Subtitle2 = new Subtitle2()
            {
                FontFamily = DefaultFontFamily,
                FontSize = DefaultFontSize,
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = "0"
            }
        };
        Shadows = new Shadow();
        ZIndex = new ZIndex();
    }
}