using Microsoft.OpenApi.Models;
using MinhasReceitasApp.API.Converters;
using MinhasReceitasApp.API.Filters;
using MinhasReceitasApp.API.Token;
using MinhasReceitasApp.Application;
using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Infrastructure;
using MinhasReceitasApp.Infrastructure.Extensions;
using MinhasReceitasApp.Infrastructure.MIgrations;


var builder = WebApplication.CreateBuilder(args);
const string TYPE_TOKEN = "Bearer";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(TYPE_TOKEN, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = TYPE_TOKEN
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = TYPE_TOKEN
                },
                Scheme = "oauth2",
                Name = TYPE_TOKEN,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Suporte a Locale
// app.UseMiddleware<CultureMiddleware>(); 

app.UseHttpsRedirection();
app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnviroment())
        return;

    var databaseType = builder.Configuration.DatabaseType();
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program() { }
}
