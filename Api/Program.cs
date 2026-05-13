using Api.Contracts;
using Api.Middlewares;
using Api.Repositories;
using Api.Services.ToDo;
using Api.Services.Users;
using Api.Settings;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// repos
builder.Services.Configure<RepositorySettings>(builder.Configuration.GetSection("Neon"));
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

// services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
            }
            else
            {
                // in a production build, this would include trusted domains
            }
        }
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseMiddleware<SystemMiddleware>();
app.MapControllers();

await app.RunAsync();
