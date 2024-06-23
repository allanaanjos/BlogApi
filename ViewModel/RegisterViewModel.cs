using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O Nome e Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Nome e Obrigatório")]
        [EmailAddress(ErrorMessage = "O E-mail e ìnvalido")]
        public string Email { get; set; }
    }
}