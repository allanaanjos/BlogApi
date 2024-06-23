using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O Nome e Obrigatório")]
        [EmailAddress(ErrorMessage = "O E-mail e ìnvalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string  Password { get; set; }
    }
}