using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controller
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
       
       [HttpGet("")]
       public IActionResult Get()
       {
          return Ok(new { 
             message = "Bem vindo, Api esta online"
          });
       }
    }
}