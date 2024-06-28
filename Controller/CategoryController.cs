using Blog.Data;
using Blog.Models;
using BlogApi.Extension;
using BlogApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controller
{
    [ApiController]
    public class CategoryController : ControllerBase
    {

        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromServices] IMemoryCache cache)
        {
            try
            {
                var Categories = cache.GetOrCreate("categoryCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });
                return Ok(new ResultViewModel<List<Category>>(Categories));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("05XE1 - Falha interna no servidor"));
            }
        }

        private List<Category> GetCategories(BlogDataContext context) =>
             context.Categories.ToList();

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var Category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (Category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria Não encontrada"));

                return Ok(Category);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE1 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
           [FromServices] BlogDataContext context,
           [FromBody] EditorCategoryViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

                var category = new Category
                {
                    Id = 0,
                    Name = model.Nome,
                    Slug = model.slug.ToLower(),
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE9 - Não foi possivel incluir a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X10 - Falha interna no servidor"));
            }

        }


        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
           [FromServices] BlogDataContext context,
           [FromBody] EditorCategoryViewModel model,
           [FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

                var Category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (Category == null)
                    return NotFound();

                Category.Name = model.Nome;
                Category.Slug = model.slug;

                context.Categories.Update(Category);
                await context.SaveChangesAsync();
                return Ok(Category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "05XE8 - Não foi possivel atualizar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "05X11 - Falha interna no servidor");
            }
        }


        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeletedAsync(
           [FromServices] BlogDataContext context,
           [FromRoute] int id)
        {
            try
            {
                var Category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (Category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                context.Categories.Remove(Category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(Category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE7 - Não foi possivel deletar a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X12 - Falha interna no servidor"));
            }
        }
    }
}