using BlogAPI2.Database;
using BlogAPI2.Endpoints;
using BlogAPI2.Entities;
using BlogAPI2.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:5003", "https://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy("ProductionPolicy", builder =>
    {
        builder.WithOrigins("https://www.your-remote-api-consumer.com", "https://your-remote-api-consumer2")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConfigurationHelper>();
builder.Services.AddSingleton<RequestHelper>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthorization();

var jwtSettings = builder.Configuration.GetSection("JWT");
var jwtSecret = jwtSettings.GetValue<string>("Secret");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret!))
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Blog API",
        Description = ".NET 8 Web API"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("LocalPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlogEndpoints();
app.MapImageEndpoints();
app.MapAnalyticsEndpoints();
app.MapAuthenticantionEndpoints();
app.MapExceptionEndpoints();

app.Run();
