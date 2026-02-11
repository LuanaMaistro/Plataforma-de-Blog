using System.Security.Claims;
using Blog.Application.DTOs.Postagem;
using Blog.Application.UseCases.Postagens.CriarPostagem;
using Blog.Application.UseCases.Postagens.DeletarPostagem;
using Blog.Application.UseCases.Postagens.EditarPostagem;
using Blog.Application.UseCases.Postagens.ListarPostagens;
using Blog.Application.UseCases.Postagens.ObterPostagem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim?.Value ?? "0");
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PostagemResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] bool meus_posts = false)
    {
        int? usuarioId = null;

        if (meus_posts)
        {
            var id = GetUsuarioId();
            if (id == 0)
                return Unauthorized();
            usuarioId = id;
        }

        var query = new ListarPostagensQuery(usuarioId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PostagemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var query = new ObterPostagemQuery(id);
        var result = await _mediator.Send(query);

        if (result is null)
            return NotFound(new { message = "Postagem não encontrada" });

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PostagemResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] PostagemCreateDto dto)
    {
        var usuarioId = GetUsuarioId();
        var command = new CriarPostagemCommand(dto.Titulo, dto.Conteudo, usuarioId);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PostagemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] PostagemUpdateDto dto)
    {
        var usuarioId = GetUsuarioId();
        var command = new EditarPostagemCommand(id, dto.Titulo, dto.Conteudo, usuarioId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(int id)
    {
        var usuarioId = GetUsuarioId();
        var command = new DeletarPostagemCommand(id, usuarioId);
        await _mediator.Send(command);
        return NoContent();
    }
}
