using MudBlazor;
using MudBlazor.Services;
using MudBlazorClient.Data;
using MudBlazorClient.Extensions;
using Client.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Client.Infrastructure.Extensions;

#region "Add services to the container"

var builder = WebApplication.CreateBuilder(args);
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.Services.AddAzureAuthDependencies(builder.Configuration);
builder.Services.AddClientInfrastrctureDependencies(builder.Configuration);
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices(configuration =>
{
    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
    configuration.SnackbarConfiguration.ShowCloseIcon = false;
});
var app = builder.Build();

#endregion


#region "Configure the HTTP request pipeline"
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();

#endregion