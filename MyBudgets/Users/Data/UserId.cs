namespace MyBudgets.Users.Data;

readonly record struct UserId(Guid Value)
{
    public override string ToString() => Value.ToString();
}