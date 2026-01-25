using Mediator;

namespace HomeAccounting.Common.Infrastructure.Events;

internal interface IIntegrationEventHandler<in TEvent>: INotificationHandler<TEvent> where TEvent : IIntegrationEvent;