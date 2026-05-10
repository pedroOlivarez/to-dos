using Api.Contracts;
using Api.Entities;
using Api.Repositories;
using Api.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// repos
var connectionString = builder.Configuration.GetConnectionString("neon");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    // to-do: (lol) this feels wrong
    builder.Services.AddSingleton<IBaseRepository>(
        new BaseRepository(connectionString)
    );
    builder.Services.AddSingleton<IUserRepository>(
        new UserRepository(connectionString)
    );
}

// services
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
