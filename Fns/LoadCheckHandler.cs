using System.Text.Json.Serialization;
using Fns.CheiCheck.Dto;
using Fns.Contracts;
using Fns.ProverkaCheka.Dto;
using Fns.Requests;
using Wolverine;
using Root = Fns.ProverkaCheka.Dto.Root;

namespace Fns;

static class LoadCheckCommandExtensions
{
    public static string RawCheck(this LoadCheckCommand request, string format = "yyyyMMddTHHmm")
    {
        return
            $"t={request.PurchaseDate.ToString(format)}&s={request.Sum}&fn={request.Fn}&i={request.Fd}&fp={request.Fp}&n=1";
    }
}

public record OkRespone();
public static class LoadCheckHandler
{
    public static async Task<OkRespone> Handle(LoadCheckCommand loadCheckCommand, IMessageBus messageBus,
        ICheckService checkService)
    {
        var check = await checkService.GetAsyncByRaw(new CheckRawRequest(loadCheckCommand.RawCheck()));
        var rootCheck = ValidateFnsResponse(check);
        var command = new NormalizeCheckCommand()
        {
            Fn = loadCheckCommand.Fn,
            Fd = loadCheckCommand.Fd,
            Fp = loadCheckCommand.Fp,
            S = loadCheckCommand.Sum,
            T = loadCheckCommand.PurchaseDate,
            Login = loadCheckCommand.Login,
            Products = rootCheck.Data.Json.Items.Select(item =>
                new NormalizeCheckCommand.Product(item.Name, item.Quantity, item.Price, item.Sum)
            ).ToList()
        };
        await messageBus.PublishAsync(command);
        return new OkRespone();
    }

    private static Root ValidateFnsResponse(Receipt receipt)
    {
        FnsException.ThrowIfInvalidResponse(receipt);
        if (receipt is not Root root) throw new FnsException("receipt is invalid");
        return root;
    }
}

public static class NormalizeCheckHandler
{
    public static async Task Handle(NormalizeCheckCommand normalizeCheckCommand, IMessageBus messageBus,
        IReceiptService receiptService)
    {
        var normalizedProducts = await receiptService.GetReceipt(normalizeCheckCommand.CreateQuery());
        var products = from normalizedProduct in normalizedProducts.Items
            join fnsProduct in normalizeCheckCommand.Products on normalizedProduct.InitialRequest equals fnsProduct.Name
            select new StoreCheckCommand.Product(
                fnsProduct.Name,
                fnsProduct.Quantity,
                fnsProduct.Price,
                fnsProduct.Sum,
                normalizedProduct.Category.SecondLevelCategory,
                normalizedProduct.Category.FirstLevelCategory
                )
                ;
        var storeCheckCommand = new StoreCheckCommand(
            normalizeCheckCommand.Login,
            normalizeCheckCommand.T,
            normalizeCheckCommand.Fn,
            normalizeCheckCommand.Fd,
            normalizeCheckCommand.Fp,
            normalizeCheckCommand.S,
            products.ToList()
        );
        
        await messageBus.PublishAsync(storeCheckCommand);
    }

    private static Query CreateQuery(this NormalizeCheckCommand normalizeCheckCommand) => new Query(
        normalizeCheckCommand.Products.Select(p => p.Name).ToList()
    );
}