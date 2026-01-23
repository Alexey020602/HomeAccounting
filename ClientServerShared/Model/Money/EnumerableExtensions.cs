namespace ClientServerShared.Model.Money;

public static class EnumerableExtensions
{
    extension<TSource>(IEnumerable<TSource> source)
    {
        public Money Sum(Func<TSource, Money> selector) => source
            .Aggregate(
                Money.Zero,
                (accumulatedMoney, tSource) => accumulatedMoney + selector(tSource)
            );
    }
}