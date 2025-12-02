using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
    {
        var usuarios = await _repo.GetAllAsync(ct);
        return usuarios.Select(MapToReadDto);
    }

    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        return usuario is null ? null : MapToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
    {
        var emailNormalizado = dto.Email.Trim().ToLowerInvariant();

        if (await _repo.EmailExistsAsync(emailNormalizado, ct))
            throw new InvalidOperationException("Email já cadastrado.");

        if (CalcularIdade(dto.DataNascimento) < 18)
            throw new InvalidOperationException("Usuário deve ter pelo menos 18 anos.");

        var usuario = new Usuario
        {
            Nome = dto.Nome.Trim(),
            Email = emailNormalizado,
            Senha = dto.Senha, // em produção: hash
            DataNascimento = dto.DataNascimento,
            Telefone = dto.Telefone,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _repo.AddAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return MapToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct)
                      ?? throw new KeyNotFoundException("Usuário não encontrado.");

        var novoEmail = dto.Email.Trim().ToLowerInvariant();

        // se mudou o email, verifica duplicidade
        if (!string.Equals(usuario.Email, novoEmail, StringComparison.OrdinalIgnoreCase) &&
            await _repo.EmailExistsAsync(novoEmail, ct))
        {
            throw new InvalidOperationException("Email já cadastrado.");
        }

        if (CalcularIdade(dto.DataNascimento) < 18)
            throw new InvalidOperationException("Usuário deve ter pelo menos 18 anos.");

        usuario.Nome = dto.Nome.Trim();
        usuario.Email = novoEmail;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Telefone = dto.Telefone;
        usuario.Ativo = dto.Ativo;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repo.UpdateAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return MapToReadDto(usuario);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);

        if (usuario is null)
            return false;

        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repo.RemoveAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct)
    {
        var normalizado = email.Trim().ToLowerInvariant();
        return await _repo.EmailExistsAsync(normalizado, ct);
    }

    private static UsuarioReadDto MapToReadDto(Usuario u)
        => new(u.Id, u.Nome, u.Email, u.DataNascimento, u.Telefone, u.Ativo, u.DataCriacao);

    private static int CalcularIdade(DateTime dataNasc)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNasc.Year;
        if (dataNasc.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}
