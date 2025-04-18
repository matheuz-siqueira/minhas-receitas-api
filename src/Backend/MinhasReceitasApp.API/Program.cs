using MinhasReceitasApp.API.Filters;
using MinhasReceitasApp.Application;
using MinhasReceitasApp.Infrastructure; 


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

app.Run();

