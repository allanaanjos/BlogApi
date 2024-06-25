using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModel
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Imagem é Obrigatória")]
        public string Base64Image { get; set; }
    }
}