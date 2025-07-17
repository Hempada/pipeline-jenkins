using WebUi.Theming;
using Xunit;

namespace WebApiTests.Tests
{
    public class ThemeTests
    {
        private readonly Theme _theme;
        private static readonly string[] expectedFontFamily = ["ITCAvantGardePro-Md", "Arial", "sans-serif"];

        public ThemeTests()
        {
            _theme = Theme.Instance;
        }

        [Fact]
        public void Theme_ShouldHaveCorrectPrimaryColorInLightPalette()
        {
            // Assert: Verifica se a cor primária da paleta clara está correta
            Assert.Equal(Colors.Blue.Light, _theme.PaletteLight.Primary);
        }

        [Fact]
        public void Theme_ShouldHaveCorrectSecondaryColorInLightPalette()
        {
            // Assert: Verifica se a cor secundária da paleta clara está correta
            Assert.Equal(Colors.Fuchsia.Light, _theme.PaletteLight.Secondary);
        }

        [Fact]
        public void Theme_ShouldHaveCorrectTextPrimaryColorInDarkPalette()
        {
            // Assert: Verifica se a cor do texto primário da paleta escura está correta
            Assert.Equal(Colors.Shades.White, _theme.PaletteDark.TextPrimary);
        }

        [Fact]
        public void Theme_ShouldHaveDefaultFontFamily()
        {
            // Assert: Verifica se a família de fontes padrão está correta
            Assert.Equal(expectedFontFamily, _theme.Typography.Default.FontFamily);
        }

        [Fact]
        public void Theme_ShouldHaveDefaultFontSize()
        {
            // Assert: Verifica se o tamanho da fonte padrão está correto
            Assert.Equal(".875rem", _theme.Typography.Default.FontSize);
        }

        [Fact]
        public void Theme_ShouldBeSingleton()
        {
            // Arrange & Act: Obtém duas instâncias do tema
            var instance1 = Theme.Instance;
            var instance2 = Theme.Instance;

            // Assert: Verifica se ambas as instâncias são a mesma
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void Theme_ShouldHaveCorrectDefaultBorderRadius()
        {
            // Assert: Verifica se a borda padrão está correta
            Assert.Equal("3px", _theme.LayoutProperties.DefaultBorderRadius);
        }

        [Fact]
        public void Theme_ShouldHaveCorrectHoverOpacityInLightPalette()
        {
            // Assert: Verifica se a opacidade do hover na paleta clara está correta
            Assert.Equal(0.15, _theme.PaletteLight.HoverOpacity);
        }

        [Fact]
        public void Theme_ShouldHaveCorrectHoverOpacityInDarkPalette()
        {
            // Assert: Verifica se a opacidade do hover na paleta escura está correta
            Assert.Equal(0.25, _theme.PaletteDark.HoverOpacity);
        }
    }
}
