namespace ArquiveSe.Application.Models.Commands.Abstractions;

public interface IIdempotencyCalculator
{
    string GetIdempotency();
}
