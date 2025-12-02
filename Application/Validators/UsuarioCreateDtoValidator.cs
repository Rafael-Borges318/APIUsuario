using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UsuarioCreateDtoValidator : AbstractValidator<UsuarioCreateDto>
{
    public UsuarioCreateDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter ao menos 6 caracteres.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
            .Must(TerMaioridade).WithMessage("Usuário deve ter pelo menos 18 anos.");

        RuleFor(x => x.Telefone)
            .Matches(@"\(\d{2}\)\s?\d{4,5}-\d{4}")
            .When(x => !string.IsNullOrWhiteSpace(x.Telefone))
            .WithMessage("Telefone deve estar no formato (XX) XXXXX-XXXX.");
    }

    private bool TerMaioridade(DateTime data)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - data.Year;
        if (data.Date > hoje.AddYears(-idade)) idade--;
        return idade >= 18;
    }
}
