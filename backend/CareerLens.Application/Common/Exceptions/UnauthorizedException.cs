namespace CareerLens.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Bu işlem için yetkiniz yok.") : base(message) { }
}
