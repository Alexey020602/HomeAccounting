namespace MyBudgets.Common.Model;

internal sealed record ReceiptFiscalData(string Fn, string Fd, string Fp)
{
    public string Raw => $"fn={Fn}&i={Fd}&fp={Fp}&n=1";
};