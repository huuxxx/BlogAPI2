using BlogApi2.Data;
using BlogApi2.Repositories;
using Microsoft.OpenApi.Models;

namespace BlogApi2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IBlogContext, BlogContext>();

            builder.Services.AddScoped<IBlogRepository, BlogRepository>();

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