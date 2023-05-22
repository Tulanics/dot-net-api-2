using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Cinema
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "O Campo de nome é Obrigatório. ")]
        public string Nome { get; set; }
    }
}
