using Mediator;

namespace Authorization.Core.CheckLoginExist;

public class CheckLoginExistQuery: IQuery<bool>
{
    public required string Login { get; set; }
}