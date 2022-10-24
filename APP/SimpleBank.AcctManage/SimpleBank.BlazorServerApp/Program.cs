using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.BlazorServerApp.Contracts;
using SimpleBank.BlazorServerApp.Services;
using SimpleBank.BlazorServerApp.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<ISbLocalStorage, SbLocalStorage>();
builder.Services.AddTransient<IUserStorage, UserStorage>();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7074/api/") });
builder.Services.AddScoped<ProtectedLocalStorage>();


//builder.Services.AddHttpClient("API", client => //https://stackoverflow.com/questions/63076954/automatically-attaching-access-token-to-http-client-in-blazor-wasm
//{
//    client.BaseAddress = new Uri("https://localhost:7074/api/");
//    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//    //client.DefaultRequestHeaders.Authorization
//});//.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();





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
