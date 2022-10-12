using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Core.Models;
using SimpleBankAPI.Core.BuildExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true);

builder.Services.AddEndpointsApiExplorer();

//log
builder.Host.RegisterSerilog();

//ef context
builder.Services.AddDbContext<SimpleBankAPIContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

//app infrastructure
builder.Services.RegisterRepos();
builder.Services.RegisterBusinesses();
builder.Services.RegisterProviders();

//auth n swagger
builder.Services.AddMyAuthenthication(builder.Configuration); 
builder.Services.AddMySwagger();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
    app.MapControllers()
);

app.Run();
