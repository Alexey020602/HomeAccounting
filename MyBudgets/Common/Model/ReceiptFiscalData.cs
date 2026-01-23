using ClientServerShared.Model;
using ClientServerShared.Model.Dates;
using ClientServerShared.Model.Money;

namespace MyBudgets.Common.Model;

internal sealed record ReceiptFiscalData
{
    private ReceiptFiscalData(string Fn, string Fd, string Fp)
    {
        this.Fn = Fn;
        this.Fd = Fd;
        this.Fp = Fp;
    }

    public static ReceiptFiscalData Create(string fn, string fd, string fp)
    {
        if (fn.Length != 16)
        {
            throw new ArgumentException("fn length must be 16");
        }

        if (fd.Length is > 5 or < 3)
        {
            throw new ArgumentException("fd must be between 3 and 5");
        }

        if (fp.Length is > 10 or < 8)
        {
            throw new ArgumentException("fp must be between 10 and 8");
        } 
        
        
        if (!fn.All(char.IsDigit))
        {
            throw new ArgumentException("fn must be digit");
        }

        if (!fd.All(char.IsDigit))
        {
            throw new ArgumentException("fd must be digit");
        }

        if (!fp.All(char.IsDigit))
        {
            throw new ArgumentException("fp must be digit");
        }
        
        return new ReceiptFiscalData(fn, fd, fp);
    }

    public static ReceiptFiscalData Empty()
    {
        // Возвращаем валидный объект с нулевыми значениями
        return new ReceiptFiscalData("0000000000000000", "000", "00000000");
    }

    public string Fn { get; }
    public string Fd { get; }
    public string Fp { get; }
    public string Raw => $"fn={Fn}&i={Fd}&fp={Fp}&n=1";

    public void Deconstruct(out string fn, out string fd, out string fp)
    {
        fn = Fn;
        fd = Fd;
        fp = Fp;
    }
};

internal sealed record FullReceiptFiscalData
{
    public static FullReceiptFiscalData Create(Money sum, DateTimeOffset purchaseDate, ReceiptFiscalData fiscalData)
    {
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future");
        }
        
        return new FullReceiptFiscalData(sum, purchaseDate, fiscalData);
    }
    private FullReceiptFiscalData(Money Sum, DateTimeOffset PurchaseDate, ReceiptFiscalData FiscalData)
    {
        this.Sum = Sum;
        this.PurchaseDate = PurchaseDate;
        this.FiscalData = FiscalData;
    }
    public Money Sum { get; init; }
    public DateTimeOffset PurchaseDate { get; init; }
    public ReceiptFiscalData FiscalData { get; init; }
    public string GetRawString(string format = "yyyyMMddTHHmm") => $"{FiscalData.Raw}&t={PurchaseDate.ToString(format)}&s={Sum}";
    public void Deconstruct(out Money sum, out DateTimeOffset purchaseDate, out ReceiptFiscalData fiscalData)
    {
        sum = Sum;
        purchaseDate = PurchaseDate;
        fiscalData = FiscalData;
    }
}