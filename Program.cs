using MongoDB.Driver;
using vita_care.Models;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
DotNetEnv.Env.Load();

// Add services to the container.
var mongoSettings = new MongoDbSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? throw new InvalidOperationException("MONGODB_CONNECTION_STRING not found"),
    DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? throw new InvalidOperationException("MONGODB_DATABASE_NAME not found")
};

builder.Services.AddSingleton(mongoSettings);
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoSettings.ConnectionString));

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register Repositories
builder.Services.AddScoped<vita_care.Repositories.IUserRepository, vita_care.Repositories.UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
