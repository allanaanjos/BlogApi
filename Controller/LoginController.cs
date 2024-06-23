using Blog.Data;
using Blog.Models;
using BlogApi.Extension;
using BlogApi.Services;
using BlogApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogApi.Controller
{

    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("v1/account")]
        public async Task<IActionResult> Accont(
            [FromServices] BlogDataContext context,
            [FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            var user = new User
            {
                Name = model.Nome,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };

            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(
                    new
                    {
                        user = user.Email,
                        password
                    }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05X99 - Este E-mail ja esta cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }



        [HttpPost("v1/account/login")]
        public async Task<IActionResult> Login(
            [FromServices] TokenService tokenService,
            [FromBody] LoginViewModel model,
            [FromServices] BlogDataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            var user = await context.Users
              .AsNoTracking()
              .Include(x => x.Roles)
              .FirstOrDefaultAsync(x => x.Email == model.Email);

            if(user == null)
               return StatusCode(400, new ResultViewModel<string>("Usuário ou senha invalido!"));
            
            if(!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(400, new ResultViewModel<string>("Usuário ou senha invalido!"));

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }

    }
}