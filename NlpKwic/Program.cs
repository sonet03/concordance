using Collocation;
using Lemmatizer;
using NlpKwic.Components;
using NlpKwic.Components.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddScoped<ILemmatizerService, LemmatizerService>()
    .AddScoped<ConcordanceService>()
    .AddScoped<IStopWordService, StopWordService>()
    .AddScoped<CollocationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();