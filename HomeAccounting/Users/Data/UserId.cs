using System.ComponentModel;

namespace HomeAccounting.Users.Data;

readonly record struct UserId(Guid Value)
{
    public override string ToString() => Value.ToString();
    
    
}