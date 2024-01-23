using BlogApi2.Data;
using BlogApi2.Models;
using BlogApi2.Repositories;
using BlogApi2.Services;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace BlogApi2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

            //builder.Services.AddScoped<IBlogContext, BlogContext>();

            //var client = new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            //var database = client.GetDatabase(builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            //builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));

            //builder.Services.AddScoped<IBlogRepository, BlogRepository>();

            //builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase"));

            builder.Services.Configure<BlogDatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

            builder.Services.AddSingleton<BlogService>();

            builder.Services.AddMvcCore().AddApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog API", Version = "v1" });
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
            });

            app.MapControllers();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}