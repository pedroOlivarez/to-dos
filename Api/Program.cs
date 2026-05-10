using Api.Contracts;
using Api.Repositories;
using Api.Services.Users;
using Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// repos
builder.Services.Configure<RepositorySettings>(
builder.Configuration.GetSection("Neon"));
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


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
