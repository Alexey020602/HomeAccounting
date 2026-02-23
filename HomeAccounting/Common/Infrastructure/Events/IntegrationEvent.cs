namespace HomeAccounting.Common.Infrastructure.Events;

abstract record IntegrationEvent(Guid Id, DateTimeOffset OccurredDateTime): IIntegrationEvent;