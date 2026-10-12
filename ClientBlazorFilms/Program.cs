using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientBlazorFilms;
using ClientBlazorFilms.Services;
using ClientBlazorFilms.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7235/")
});

builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddScoped<FilmsViewModel>();
builder.Services.AddScoped<FilmDetailViewModel>();

await builder.Build().RunAsync();