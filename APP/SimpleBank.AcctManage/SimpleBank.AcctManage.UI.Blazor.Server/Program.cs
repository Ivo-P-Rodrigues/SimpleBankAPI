using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Net.Http.Headers;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Clients;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services;
using SimpleBank.AcctManage.UI.Blazor.Server.Providers;
using SimpleBank.AcctManage.UI.Blazor.Server.Services;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.HttpClients;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddLocalization();
builder.Services.AddControllers();

builder.Services.AddHttpClient("SbApi", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SbApi:BaseUrl"));
    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Services.AddHttpClient<IAuthClient, AuthClient>(authClient =>
{
    authClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SbApi:AuthUrl"));
    authClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ApiAuthenticationStateProvider>());
builder.Services.AddScoped<JwtSecurityTokenHandler>();

builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<ISimpleBankClient, SimpleBankClient>();
builder.Services.AddTransient<IEntityMapper, EntityMapper>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountDocService, AccountDocService>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<IMovementService, MovementService>();

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
