# HomeAccounting

Приложение для учёта и анализа личного бюджета. Пользователь фиксирует траты на основе данных чека или введённых вручную данных в рамках бюджетов, в которых он принимает участие.

Проект также является обучающим и развивается с оглядкой на [evolutionary-architecture-by-example](https://github.com/evolutionary-architecture/evolutionary-architecture-by-example).

## Возможности

- Создание бюджетов с лимитами и расчётными периодами
- Совместная работа: приглашение участников с ролями (Owner, Admin, User)
- Ручной учёт трат с категориями
- Добавление чеков по фискальным данным, QR-коду или файлу
- Автоматическая обработка чеков через ФНС с retry-логикой
- Иерархический справочник категорий
- Аутентификация через JWT + refresh token с ротацией

## Стек

.NET Aspire, ASP.NET Core (Minimal API), Blazor WebAssembly, MudBlazor, EF Core, PostgreSQL, Refit, Mediator, Serilog, Scalar, xUnit

## Запуск

Подробнее: [docs/development/local-development.md](docs/development/local-development.md)

```bash
dotnet run --project AppHost
```

AppHost через .NET Aspire поднимает PostgreSQL, PgAdmin, бэкенд. Миграции применяются автоматически.

## Структура проекта

| Проект | Назначение |
|---|---|
| **AppHost** | Aspire AppHost: оркестрация PostgreSQL, PgAdmin |
| **HomeAccounting** | ASP.NET Core бэкенд: Minimal API, EF Core, бизнес-логика |
| **Client** | Blazor WebAssembly — точка входа |
| **BlazorConsolidated** | Blazor UI: компоненты, страницы, Refit-клиенты |
| **ClientServerContracts** | Shared DTO: запросы, ответы |
| **ClientServerContracts.Api** | Refit-интерфейсы API-клиентов |
| **ClientServerShared** | Общая логика: валидация, QR, results |
| **ServiceDefaults** | Aspire service defaults |

## Документация

- [Обзор проекта](docs/project-overview.md) — цель, модель данных, bounded contexts, правила
- **Модули:**
  [Budgets](docs/modules/budgets.md) |
  [Users](docs/modules/users.md) |
  [Categories](docs/modules/categories.md) |
  [Receipt Processing](docs/modules/receipt-processing.md)
- **Архитектура:**
  [Обзор](docs/architecture/architecture-overview.md) |
  [Аутентификация](docs/architecture/auth.md) |
  [Персистентность](docs/architecture/persistence.md)
- [Локальная разработка](docs/development/local-development.md)
- [Архитектурные решения (ADR)](docs/decisions/)
- [Спецификации](docs/specs/)

## В планах

- SPA-клиент на Angular
- Telegram-бот
- Мобильное (и десктопное) приложение
- [Система уведомлений](docs/specs/notifications-requirements.md) (SignalR, in-app)
