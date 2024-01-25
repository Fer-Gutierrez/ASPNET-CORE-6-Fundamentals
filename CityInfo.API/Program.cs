using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Profiles;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

//Configuramos SERILOG:
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//Para condigurar el Logger utilizado por la aplicacion:
//builder.Logging.ClearProviders();       //limpiamos los proovedores de Loggin que se crean en el metodo CreateBuilder (CreateDefaultBuilder).
//builder.Logging.AddConsole();           //agregamos una consola nueva

//Agregamos SERILOG como logger de la app:
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true; //Devuelve un 406 No Acceptable si nos piden un Content Negotiation distinto al configurado
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); // Agrego que voy a utilizar NewtonsoftJson y que permito XML en la propiedad de Accept Header (Content Negotiation).

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    opt.IncludeXmlComments(xmlCommentsFullPath);

    opt.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth"
                }
            }, new List<string>()
        }
    });
});
builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); //Para poder obtener los tipos de contenido que necesitan los achivos para ser
//enviados o descargados.

//Servicion de Mail (Iyeccion de dependencias):
//Definimos el servicio de email utilizado segun el compiling mode - tambien podria definirse segun el environmentName -
#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CityDataStore>();

builder.Services.AddDbContext<CityInfoContext>(opt => opt.UseSqlServer(builder.Configuration["ConnectionStrings:CityInfoDB"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"] ?? "https://localhost:7102",
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Authentication:Audience"] ?? "cityInfoApi",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"] ?? "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)"))

    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromRafaela", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Rafaela");
    });
});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

//app.MapControllers();

app.Run();
