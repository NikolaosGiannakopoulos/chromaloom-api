using Figgle.Fonts;
using Spectre.Console;

namespace ChromaLoom.Api.Setup;

internal static partial class ServiceRegistration
{
    extension(WebApplicationBuilder builder)
    {
        private WebApplicationBuilder AddStartupBanner()
        {
            var content = new Rows(
                new Text(FiggleFonts.KeyboardSmall.Render("Chroma"), new Style(Color.Cyan)),
                new Text(FiggleFonts.KeyboardSmall.Render("Loom"), new Style(Color.Cyan)),
                new Text("Connecting every thread of your craft \nin one woven powerhouse.", new Style(Color.Cyan))
            );

            var panel = new Panel(content)
            {
                Border = BoxBorder.Double,
                BorderStyle = new Style(Color.Cyan),
                Expand = false
            };

            AnsiConsole.Write(panel);

            return builder;
        }
    }
}
