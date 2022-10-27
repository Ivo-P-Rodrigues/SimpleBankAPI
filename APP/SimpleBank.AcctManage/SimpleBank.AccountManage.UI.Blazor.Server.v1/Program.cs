using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Services;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddLocalization();
builder.Services.AddControllers();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<ISbLocalStorage, SbLocalStorage>();
builder.Services.AddTransient<IUserStorage, UserStorage>();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7074/api/") });
builder.Services.AddScoped<ProtectedLocalStorage>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


var supportedCultures = new[] { "en-US", "pt-PT" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
