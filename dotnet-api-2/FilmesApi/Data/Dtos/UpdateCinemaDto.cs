using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos
{
    public class UpdateCinemaDto
    {
        [Required(ErrorMessage = "O Campo de nome é Obrigatório. ")]
        public string Nome { get; set; }
    }
}
