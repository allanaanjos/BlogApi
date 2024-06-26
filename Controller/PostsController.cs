using Blog.Data;
using Blog.Models;
using BlogApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controller
{
    [ApiController]
    public class PostsController : ControllerBase
    {

        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetPostsAsync(
            [FromServices] BlogDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            var posts = await context.Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Select(x => new ListPostViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} - ({x.Author.Email})",
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderBy(x => x.LastUpdateDate)
                    .ToListAsync();

            try
            {

                return Ok(new ResultViewModel<dynamic>(new
                {
                    page,
                    pageSize,
                    posts
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("50XE4 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> GetPostIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var post = await context.Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .ThenInclude(x => x.Roles)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

                if (post == null)
                    return NotFound(new ResultViewModel<Post>("Post não encontrado"));

                return Ok(new ResultViewModel<Post>(post));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("50XE4 - Falha interna no servidor"));
            }
        }


        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] string category,
            [FromRoute] int page = 0,
            [FromRoute] int pageSize = 25)
        {

            try
            {
                var postCategory = await context.Posts
                  .AsNoTracking()
                  .Include(x => x.Author)
                  .Include(x => x.Category)
                  .Where(x => x.Category.Slug == category)
                  .Select(x => new ListPostViewModel
                  {
                      Id = x.Id,
                      Title = x.Title,
                      slug = x.Slug,
                      LastUpdateDate = x.LastUpdateDate,
                      Category = x.Category.Name,
                      Author = $"{x.Author.Name} - ({x.Author.Email})"
                  })
                  .Skip(page * pageSize)
                  .Take(pageSize)
                  .OrderByDescending(x => x.LastUpdateDate)
                  .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    page,
                    pageSize,
                    postCategory
                }));

            }
            catch (Exception)
            {
               return StatusCode(500, new ResultViewModel<Post>("50XE4 - Falha interna no servidor"));
            }
        }
    
    
        
    }
}