using MongoDB.Driver;
using vita_care.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

// Register Repositories
builder.Services.AddScoped<vita_care.Repositories.IUserRepository, vita_care.Repositories.UserRepository>();
builder.Services.AddScoped<vita_care.Repositories.IDoctorRepository, vita_care.Repositories.DoctorRepository>();
builder.Services.AddScoped<vita_care.Repositories.IAppointmentRepository, vita_care.Repositories.AppointmentRepository>();

// Configure Authentication
var auth0Domain = $"https://{Environment.GetEnvironmentVariable("AUTH0_DOMAIN")}/";
var auth0Audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = auth0Domain;
        options.Audience = auth0Audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
        
        // Handle challenges specifically to return 401 or 403 as requested
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Disables the default "WWW-Authenticate" header being added
                context.HandleResponse();
                
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"Message\": \"Unauthorized. Please provide a valid token.\"}");
            },
            OnForbidden = context =>
            {
                 context.Response.StatusCode = StatusCodes.Status403Forbidden;
                 context.Response.ContentType = "application/json";
                 return context.Response.WriteAsync("{\"Message\": \"Forbidden. You do not have permission to access this resource.\"}");
            }
        };
    });

// Register FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("VitaCareCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
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

app.UseCors("VitaCareCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
