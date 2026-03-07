# Персистентность

## Обзор

Для хранения данных используется PostgreSQL с доступом через Entity Framework Core. Каждый bounded context имеет свой `DbContext` со своей схемой БД.

## Контексты данных

| DbContext | Схема | Модуль | Описание |
|---|---|---|---|
| `UsersContext` | `Identity` | Users | Пользователи, refresh-токены (ASP.NET Core Identity) |
| `BudgetsContext` | `budgets` | Budgets | Бюджеты, участники, операции, чеки, товары, outbox |
| `CategoriesContext` | `categories` | Categories | Категории и иерархия |

Каждый контекст регистрируется через общий extension `AddDbContext<TContext>()` из `HomeAccounting.Common.Infrastructure.Database.Extensions`.

## Подключение к БД

- Провайдер: **Npgsql** (`Aspire.Npgsql.EntityFrameworkCore.PostgreSQL`)
- Имя ресурса Aspire: `"homeaccounting-db"`
- Подключение настраивается через Aspire service discovery: `AddNpgsqlDbContext<TContext>(serviceName)`
- Дополнительно используется `EntityFramework.Exceptions.PostgreSQL` (`UseExceptionProcessor()`) для типизированной обработки ошибок БД

## Миграции

### Стратегия

Миграции применяются автоматически при старте приложения. В `Program.cs`:

```csharp
await app.MigrateUsersAsync();
await app.MigrateBudgetsAsync();
await app.MigrateCategoriesAsync();
```

Общая логика: `AutomaticMigrationsExtension.MigrateDatabaseAsync<TContext>()` — вызывает `context.Database.MigrateAsync()`.

### Таблица истории миграций

Каждый контекст хранит историю миграций (`__EFMigrationsHistory`) в своей схеме.

### Правила для разработки

- Каждый bounded context управляет своими миграциями независимо.
- Миграции создаются стандартными инструментами EF Core (`dotnet ef migrations add`).
- Автоматическое применение при старте подходит для текущего этапа разработки. При переходе к production стоит рассмотреть управляемое применение миграций.

## Конфигурация в Aspire

PostgreSQL разворачивается через AppHost:

- Контейнер с `ContainerLifetime.Persistent` (не пересоздаётся при рестарте)
- Data volume для персистентности данных
- PgAdmin для администрирования (также persistent)
- Параметры подключения (`Username`, `Password`) — Aspire secrets
