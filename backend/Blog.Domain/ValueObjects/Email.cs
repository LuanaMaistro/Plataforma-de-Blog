using Blog.Domain.Exceptions;

namespace Blog.Domain.ValueObjects;

public class Email
{
    public string Valor { get; }

    private Email() { } // EF Core

    public Email(string email)
    {
        if (!IsValid(email))
            throw new DomainException("Email invalido");
        Valor = email.ToLowerInvariant();
    }

    private static bool IsValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var trimmedEmail = email.Trim();
        if (trimmedEmail.EndsWith("."))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is Email other)
            return Valor == other.Valor;
        return false;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor;

    public static implicit operator string(Email email) => email.Valor;
}
