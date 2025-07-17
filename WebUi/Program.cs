using Commons.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using WebUi;
using WebUi.Extensions;
using static WebUi.App;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure(builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();

public static class AppData
{
    public const string Name = "TMS";
    public const string Version = "0.0.1";
    public static MenuItem[] MenuItems { get; } =
    [
        new MenuItem("", "Dashboard", @Icons.Material.Filled.Dashboard),
        new MenuItem("accounts", "Usuários", @Icons.Material.Filled.People, [Permission.VIEW_ACCOUNT]),
        new MenuItem("profiles", "Perfis", @Icons.Material.Filled.People, [Permission.VIEW_PROFILE]),
        new MenuItem("customers", "Clientes", @Icons.Material.Filled.People, [Permission.VIEW_CUSTOMER]),
    ];
}