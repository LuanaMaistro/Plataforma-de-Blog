using Blog.Domain.Exceptions;

namespace Blog.Domain.ValueObjects;

public class SenhaHash
{
    public string Valor { get; }

    private SenhaHash() { } // EF Core

    public SenhaHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException("Hash de senha invalido");
        Valor = hash;
    }

    public override bool Equals(object? obj)
    {
        if (obj is SenhaHash other)
            return Valor == other.Valor;
        return false;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => "****";

    public static implicit operator string(SenhaHash senha) => senha.Valor;
}
