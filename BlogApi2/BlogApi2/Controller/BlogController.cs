using BlogApi2.Entities;
using BlogApi2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApi2.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repository;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IBlogRepository repository, ILogger<BlogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Blog>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            var blogs = await _repository.GetBlogs();
            return Ok(blogs);
        }

        [HttpGet("{id:length(24)}", Name = "GetBlog")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Blog), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Blog>> GetBlogById(string id)
        {
            var blog = await _repository.GetBlog(id);

            if (blog == null)
            {
                _logger.LogError($"Blog with id: {id}, not found.");
                return NotFound();
            }

            return Ok(blog);
        }

        [Route("[action]/{title}", Name = "GetBlogByTitle")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Blog>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogByName(string name)
        {
            var items = await _repository.GetBlogByTitle(name);
            if (items == null)
            {
                _logger.LogError($"Blogs with name: {name} not found.");
                return NotFound();
            }
            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Blog), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Blog>> CreateBlog([FromBody] Blog blog)
        {
            await _repository.CreateBlog(blog);

            return CreatedAtRoute("GetBlog", new { id = blog.Id }, blog);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Blog), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBlog([FromBody] Blog blog)
        {
            return Ok(await _repository.UpdateBlog(blog));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteBlog")]
        [ProducesResponseType(typeof(Blog), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBlogById(string id)
        {
            return Ok(await _repository.DeleteBlog(id));
        }
    }
}
