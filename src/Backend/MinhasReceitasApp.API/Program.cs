using MinhasReceitasApp.API.Filters;
using MinhasReceitasApp.Application;
using MinhasReceitasApp.Infrastructure;
using MinhasReceitasApp.Infrastructure.Extensions;
using MinhasReceitasApp.Infrastructure.MIgrations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter))); 
builder.Services.AddApplication(builder.Configuration); 
builder.Services.AddInfrastructure(builder.Configuration);


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
    if(builder.Configuration.IsUnitTestEnviroment())
        return; 

    var databaseType = builder.Configuration.DatabaseType();
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();  
    DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program(){}
}
