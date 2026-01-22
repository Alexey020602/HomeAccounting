using Mediator;

namespace MyBudgets.Common.Infrastructure.Events;

internal interface IIntegrationEvent: INotification
{
    Guid Id { get; }
    DateTimeOffset OccurredDateTime { get; }
}