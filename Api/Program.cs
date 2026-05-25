using System.Text;
using Api.Contracts;
using Api.Middlewares;
using Api.Repositories;
using Api.Services.ToDo;
using Api.Services.User;
using Api.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// repos
builder.Services.Configure<RepositorySettings>(builder.Configuration.GetSection("Neon"));
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.JwtOptionsKey)
);
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoService, ToDoService>();

builder
    .Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        var clientId =
            builder.Configuration["Authentication:Google:ClientId"]
            ?? throw new ArgumentNullException("Google ClientId");
        var clientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"]
            ?? throw new ArgumentNullException("Google ClientSecret");

        options.ClientId = clientId;
        options.ClientSecret = clientSecret;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtOptions =
            builder.Configuration.GetSection(JwtSettings.JwtOptionsKey).Get<JwtSettings>()
            ?? throw new ArgumentException(nameof(JwtSettings));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["ACCESS_TOKEN"];
                return Task.CompletedTask;
            },
        };
    });

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
