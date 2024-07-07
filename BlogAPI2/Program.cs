using Microsoft.EntityFrameworkCore;
using BlogAPI2.Database;
using BlogAPI2.Extensions;
using BlogAPI2.Endpoints;
using Microsoft.AspNetCore.Rewrite;
using BlogAPI2.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConfigurationHelper>();
builder.Services.AddSingleton<RequestHelper>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.ApplyMigrations();
}

app.UseSwagger();
app.UseSwaggerUI();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option); // redirect index to swagger

app.MapBlogEndpoints();
app.MapImageEndpoints();
app.MapAnalyticsEndpoints();

app.Run();
