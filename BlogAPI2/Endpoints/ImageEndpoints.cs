using BlogAPI2.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI2.Endpoints
{
    public static class ImageEndpoints
    {
        public static void MapImageEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("images", async ([FromForm] IFormFile file, CancellationToken ct, ConfigurationHelper configurationHelper) =>
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string fileExtension = Path.GetExtension(file.FileName);
                string ImagesDirectory = configurationHelper.GetImagesDirectory();
                
                string path = Path.Combine(Directory.GetCurrentDirectory(), @$"{ImagesDirectory}/" + timeStamp + fileExtension);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream, ct);
                }

                string apiUrl = configurationHelper.GetApiUrl();
                string outputUrl = $"{apiUrl}/{ImagesDirectory}/{timeStamp + fileExtension}";
                return Results.Ok(outputUrl);
            })
            .DisableAntiforgery().RequireAuthorization();

            app.MapDelete("images/{id}", (string id, ConfigurationHelper configurationHelper) =>
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $@"{configurationHelper.GetImagesDirectory}\" + id);
                File.Delete(path);
                return Results.Ok();
            });

            app.MapGet("images", (ConfigurationHelper configurationHelper) =>
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"{configurationHelper.GetImagesDirectory}");
                string[] images = Directory.GetFiles(path);

                if (images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        images[i] = Path.GetFileName(images[i]);
                    }
                }

                return Results.Ok(images);
            })
            .RequireAuthorization();
        }
    }
}
