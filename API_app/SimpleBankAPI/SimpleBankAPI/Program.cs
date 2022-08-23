using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleBankAPI.Models;
using SimpleBankAPI.Options;
using SimpleBankAPI.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//context
builder.Services.AddDbContext<SimpleBankAPIContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

//app infrastructure
builder.Services.RegisterRepos();
builder.Services.RegisterBusinesses();
builder.Services.Miscellaneous();

//auth
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"]))
        };
    });


//swagger and versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; //allow versioning
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(options =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //add xml documentation to swagger
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    options.IncludeXmlComments(xmlCommentsFullPath);

    options.AddSecurityDefinition("SimpleBankAPIBearerAuth", new OpenApiSecurityScheme() //add security to swagger
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description ="Input a valid token to access this API."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement //set token in request headers   //Dict with OpenApiSecurityScheme as key
    {
        {
            new OpenApiSecurityScheme() 
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "SimpleBankAPIBearerAuth" }
            }, new List<string>() }
    });


    options.OperationFilter<SwaggerDefaultValues>();
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();




var app = builder.Build();


var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"SimpleBankAPI - {description.GroupName.ToUpper()}");
        }
    });
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
    app.MapControllers()
);

app.Run();
