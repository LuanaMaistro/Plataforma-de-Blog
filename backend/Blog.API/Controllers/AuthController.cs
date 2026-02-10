using System.Security.Claims;
using Blog.Application.DTOs.Usuario;
using Blog.Application.UseCases.Auth.AtualizarPerfil;
using Blog.Application.UseCases.Auth.LoginUsuario;
using Blog.Application.UseCases.Auth.RegistrarUsuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto dto)
    {
        var command = new RegistrarUsuarioCommand(dto.Nome, dto.Email, dto.Senha);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Registrar), new { id = result.Id }, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
    {
        var command = new LoginUsuarioCommand(dto.Email, dto.Senha);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("profile")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtualizarPerfil([FromBody] AtualizarPerfilDto dto)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new AtualizarPerfilCommand(usuarioId, dto.Nome, dto.SenhaAtual, dto.NovaSenha);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
