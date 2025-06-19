namespace Fns.Contracts;

public record LoadCheckCommand(
    string Login,
    DateTime PurchaseDate,
    string Fn,
    string Fd,
    string Fp,
    string Sum
)
{
}
