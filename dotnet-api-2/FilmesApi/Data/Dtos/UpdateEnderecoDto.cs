using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos
{
    public class UpdateEnderecoDto
    {
        [Required(ErrorMessage = "O campo de nome é obrigatório. ")]
        public string Logradouro { get; set; }
        public int Numero { get; set; }
    }
}
