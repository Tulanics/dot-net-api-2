﻿using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme(
        [FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId),
            new { id = filme.Id },
            filme);
    }

    /// <summary>
    /// Recupera uma lista de filmes do banco de dados
    /// </summary>
    /// <param name="skip">Número de filmes que serão pulados</param>
    /// <param name="take">Número de filmes que serão recuperados</param>
    /// <returns>Informações dos filmes buscados</returns>
    /// <response code="200">Com a lista de filmes presentes na base de dados</response>
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
    }

    /// <summary>
    /// Recupera um filme no banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser recuperado no banco</param>
    /// <returns>Informações do filme buscado</returns>
    /// <response code="200">Caso o id seja existente na base de dados</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes
            .FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza um filme no banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado no banco</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>Sem conteúdo de retorno</returns>
    /// <response code="204">Caso o id seja existente na base de dados e o filme tenha sido atualizado</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id,
        [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza um filme no banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado no banco</param>
    /// <param name="patch">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>Sem conteúdo de retorno</returns>
    /// <response code="204">Caso o id seja existente na base de dados e o filme tenha sido atualizado</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id,
        JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme no banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado no banco</param>
    /// <returns>Sem conteúdo de retorno</returns>
    /// <response code="204">Caso o id seja existente na base de dados e o filme tenha sido atualizado</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
        