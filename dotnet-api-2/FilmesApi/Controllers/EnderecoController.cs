using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        /// <summary>
        /// construtor de classe
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um endereço ao banco de dados
        /// </summary>
        /// <param name="enderecoDto">Objeto com os campos necessários para criação de um endereço</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaEnderecoPorId),
           new { id = endereco.Id },
           endereco);
        }

        /// <summary>
        /// Recupera uma lista de endereço do banco de dados
        /// </summary>
        /// <param name="skip">Número de endereço que serão pulados</param>
        /// <param name="take">Número de endereço que serão recuperados</param>
        /// <returns>Informações dos endereço buscados</returns>
        /// <response code="200">Com a lista de endereço presentes na base de dados</response>
        [HttpGet]
        public IEnumerable<ReadEnderecoDto> RecuperaEndereco([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
        {
            var enderecos = _context.Enderecos.Skip(skip).Take(take);
            return _mapper.Map<List<ReadEnderecoDto>>(enderecos); ;
        }


        /// <summary>
        /// Recupera um endereço no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do endereço a ser recuperado no banco</param>
        /// <returns>Informações do endereço buscado</returns>
        /// <response code="200">Caso o id seja existente na base de dados</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpGet("{id}")]
        public IActionResult RecuperaEnderecoPorId(int id)
        {
            var filme = _context.Filmes
                .FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return Ok(filmeDto);
        }

        /// <summary>
        /// Recupera um endereco no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do endereco a ser recuperado no banco</param>
        /// <returns>Informações do endereco buscado</returns>
        /// <response code="200">Caso o id seja existente na base de dados</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpPut]
        public IActionResult AtualizaEndereco(int id, 
            [FromBody] UpdateEnderecoDto enderecoDto)
        {
            var endereco = _context.Enderecos.FirstOrDefault(
                endereco=> endereco.Id == id);
            if(endereco == null) return NotFound();
            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return Ok(endereco);
        }

        /// <summary>
        /// Atualiza parcialmente (algum atributo) um endereço no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do endereço a ser atualizado no banco</param>
        /// <param name="patch">Objeto com os campos necessários para atualização de um endereço</param>
        /// <returns>Sem conteúdo de retorno</returns>
        /// <response code="204">Caso o id seja existente na base de dados e o endereço tenha sido atualizado</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpPatch]
        public IActionResult AtualizaEnderecoParcial(int id,
            JsonPatchDocument<UpdateEnderecoDto> patch)
        {
            var endereco = _context.Enderecos.FirstOrDefault(
                endereco => endereco.Id == id);
            if (endereco == null) return NotFound();

            var enderecoParaAtualizar = _mapper.Map<UpdateEnderecoDto>(endereco);
            patch.ApplyTo(enderecoParaAtualizar, ModelState);
            if (TryValidateModel(enderecoParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(enderecoParaAtualizar, endereco);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deleta um endereço no banco de dados usando seu id
        /// </summary>
        /// <param name="id">Id do endereço a ser atualizado no banco</param>
        /// <returns>Sem conteúdo de retorno</returns>
        /// <response code="204">Caso o id seja existente na base de dados e o endereço tenha sido atualizado</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpDelete("{id}")]
        public IActionResult DeletaEndereco(int id)
        {
            var endereco = _context.Enderecos.FirstOrDefault(
                endereco=> endereco.Id == id);
            if (endereco == null) return NotFound();

            _context.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
