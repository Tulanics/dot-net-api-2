using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesApi.Controllers
{
    /// <summary>
    /// Classe cinema
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CinemaController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        /// <summary>
        /// construtor de classe
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public CinemaController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um cinema ao banco de dados
        /// </summary>
        /// <param name="cinemaDto">Objeto com os campos necessários para criação de um cinema</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        public IActionResult AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaCinemasPorId),
                new { Id = cinema.Id }, cinemaDto);
        }

        /// <summary>
        /// Recupera uma lista de cinemas do banco de dados
        /// </summary>
        /// <returns>Informações dos cinemas buscados</returns>
        /// <response code="200">Com a lista de cinemas presentes na base de dados</response>
        [HttpGet]
        public IEnumerable<ReadCinemaDto> RecuperaCinemas()
        {
            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.ToList());
        }

        /// <summary>
        /// Recupera um cinema no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do cinema a ser recuperado no banco</param>
        /// <returns>Informações do cinema buscado</returns>
        /// <response code="200">Caso o id seja existente na base de dados</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpGet("{id}")]
        public IActionResult RecuperaCinemasPorId(int id)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema != null)
            {
                ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
                return Ok(cinemaDto);
            }
            return NotFound();
        }

        /// <summary>
        /// Atualiza um cinema no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do cinema a ser atualizado no banco</param>
        /// <param name="cinemaDto">Objeto com os campos necessários para atualização de um cinema</param>
        /// <returns>Sem conteúdo de retorno</returns>
        /// <response code="204">Caso o id seja existente na base de dados e o cinema tenha sido atualizado</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpPut("{id}")]
        public IActionResult AtualizaCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            _mapper.Map(cinemaDto, cinema);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Atualiza parcialmente (algum atributo) um cinema no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do cinema a ser atualizado no banco</param>
        /// <param name="patch">Objeto com os campos necessários para atualização de um cinema</param>
        /// <returns>Sem conteúdo de retorno</returns>
        /// <response code="204">Caso o id seja existente na base de dados e o cinema tenha sido atualizado</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpPatch("{id}")]
        public IActionResult AtualizaCinemaParcial(int id,
            JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var cinema = _context.Filmes.FirstOrDefault(
                cinema => cinema.Id == id);
            if (cinema == null) return NotFound();

            var cinemaParaAtualizar = _mapper.Map<UpdateFilmeDto>(cinema);

            patch.ApplyTo(cinemaParaAtualizar, ModelState);

            if (!TryValidateModel(cinemaParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(cinemaParaAtualizar, cinema);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deleta um cinema no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do cinema a ser atualizado no banco</param>
        /// <returns>Sem conteúdo de retorno</returns>
        /// <response code="204">Caso o id seja existente na base de dados e o cinema tenha sido atualizado</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpDelete("{id}")]
        public IActionResult DeletaCinema(int id)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            _context.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
