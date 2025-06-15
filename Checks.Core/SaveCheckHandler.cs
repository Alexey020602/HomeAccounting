using Authorization.Contracts;
using Fns.Contracts;
using Microsoft.AspNetCore.Http;
using Shared.Model.Requests;

namespace Checks.Core;

public record SaveCheckCommand
{
    public required string Login { get; init; }
    public required string Fn { get; init; }
    public required string Fd { get; init; }
    public required string Fp { get; init; }
    public required string S { get; init; }
    public required DateTime T { get; init; }

    internal GetCheckRequest CreateGetCheckRequestFromSaveCheckRequest() => new
        (
            Fn,
            Fd, 
            Fp,
            S,
            T
            );
}

public static class SaveCheckHandler
{
    public static async Task<LoadCheckCommand?> Handle(SaveCheckCommand request, ICheckRepository repository)
    {
        if (await repository.GetCheckByRequest(request.CreateGetCheckRequestFromSaveCheckRequest()) is not null) return null;

        return new LoadCheckCommand(request.Login, request.T, request.Fn, request.Fd, request.Fp, request.S);
    }
}

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

public static class LoadCheckHandler
{
    public static async Task<StoreCheckCommand> Handle(LoadCheckCommand loadCheckCommand, ICheckSource checkSource)
    {
        var checks = await checkSource.GetCheck(new CheckRequest(
            loadCheckCommand.Fn,
            loadCheckCommand.Fd,
            loadCheckCommand.Fp,
            loadCheckCommand.Sum,
            loadCheckCommand.PurchaseDate
            ));
        return new StoreCheckCommand(
            loadCheckCommand.Login,
            loadCheckCommand.PurchaseDate,
            loadCheckCommand.Fn,
            loadCheckCommand.Fd,
            loadCheckCommand.Fp,
            loadCheckCommand.Sum,
            checks.Products.Select(p =>
                new AddCheckRequest.Product(p.Name, p.Quantity, p.Price, p.Sum, p.Subcategory, p.Category)
            ).ToList()
        );
    }
}

public record StoreCheckCommand(
    string Login,
    DateTime PurchaseDate,
    string Fn,
    string Fd,
    string Fp,
    string Sum,
    IReadOnlyList<AddCheckRequest.Product> Products
)
{
    internal AddCheckRequest ConvertToAddCheckRequest() => new
    (
        Login,
        PurchaseDate,
        Fn,
        Fd,
        Fp,
        Sum,
        Products
    );
}

public static class StoreCheckHandler
{
    public static async Task Handle(StoreCheckCommand storeCheckCommand, ICheckRepository repository)
    {

        await repository.SaveCheck(storeCheckCommand.ConvertToAddCheckRequest());
    }
}