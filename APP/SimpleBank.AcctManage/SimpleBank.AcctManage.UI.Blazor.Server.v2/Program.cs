using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var baseAddress = new Uri(builder.Configuration.GetValue<string>("SbApiEndPoints:BaseUrl"));
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = baseAddress });

builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<HttpRequestMessageBuilder>();
builder.Services.AddScoped<SbApiConnect>();
builder.Services.AddScoped<SbLocalStorage>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
