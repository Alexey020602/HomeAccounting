using ClientServerShared.Model.Money;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HomeAccounting.Budgets.Data.Database;

internal sealed class MoneyConverter(): ValueConverter<Money, long>(
    x => x.Kopecks,
    x => Money.FromKopecks(x)
);