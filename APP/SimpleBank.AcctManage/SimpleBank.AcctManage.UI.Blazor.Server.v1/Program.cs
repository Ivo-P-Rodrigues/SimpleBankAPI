using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Services;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddLocalization();
builder.Services.AddControllers();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<ISbLocalStorage, SbLocalStorage>();
builder.Services.AddTransient<IUserStorage, UserStorage>();

var baseAddress = new Uri(builder.Configuration.GetValue<string>("SbApiEndPointsAddresses:BaseUrl"));
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = baseAddress });
//builder.Services.AddHttpClient();
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
