using Microsoft.EntityFrameworkCore;
using BlogAPI2.Database;
using BlogAPI2.Extensions;
using BlogAPI2.Endpoints;
using Microsoft.AspNetCore.Rewrite;
using BlogAPI2.Helpers;
using BlogAPI2.Entities;
using Microsoft.AspNetCore.Identity;
using BlogAPI2.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConfigurationHelper>();
builder.Services.AddSingleton<RequestHelper>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddIdentityCore<User>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddIdentityCore<User>(options =>
{
    // Configure Identity options here if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseSwagger();
app.UseSwaggerUI();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option); // redirect index to swagger

app.MapBlogEndpoints();
app.MapImageEndpoints();
app.MapAnalyticsEndpoints();
app.MapAuthenticantionEndpoints();
app.MapExceptionEndpoints();

app.Run();
