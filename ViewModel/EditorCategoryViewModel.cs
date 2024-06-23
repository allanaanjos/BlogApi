using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModel
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Nome e Obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve comter entre 3 a 40 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Slug e obrigatório")]
        public string slug { get; set; }
    }
}