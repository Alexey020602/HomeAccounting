# Локальная разработка

## Предварительные требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (для PostgreSQL и PgAdmin через Aspire)

## Запуск проекта

Проект оркестрируется через .NET Aspire. Точка входа — проект **AppHost**.

### Через Rider

1. Открыть solution `HomeAccounting.sln` в Rider.
2. Выбрать конфигурацию запуска `AppHost`.
3. Нажать Run/Debug.

### Через CLI

```bash
dotnet run --project AppHost
```

### Что запускается

AppHost автоматически поднимает:

| Ресурс | Описание |
|---|---|
| **PostgreSQL** | Контейнер с БД, persistent volume |
| **PgAdmin** | Веб-интерфейс для администрирования БД |
| **HomeAccounting** | ASP.NET Core бэкенд |

Миграции БД применяются автоматически при старте бэкенда.

## Конфигурация

### appsettings.json (HomeAccounting)

Ключевые секции:

- `JwtTokenSettings` — параметры JWT (issuer, audience, ключ, время жизни токенов)
- `TokensCleanup` — интервал очистки истёкших refresh-токенов
- `ReceiptProcessingOptions` — параметры retry обработки чеков (макс. попытки, задержки)

### Aspire secrets

Параметры подключения к PostgreSQL (`Username`, `Password`) управляются через Aspire secret parameters в AppHost.

## API-документация

После запуска доступна документация API через Scalar:

- URL: `https://localhost:{port}/scalar`

## Запуск тестов

### Через Rider

Правый клик на проект тестов → Run Tests.

### Через CLI

```bash
dotnet test HomeAccounting.UnitTests
dotnet test BlazorConsolidated.Tests
```

### Структура тестов

| Проект | Покрытие |
|---|---|
| `HomeAccounting.UnitTests` | Бэкенд: пагинация, фильтрация, денежные операции, категории |
| `BlazorConsolidated.Tests` | Фронтенд: TokenService, авторизация |

## Структура solution

Подробнее о проектах и их зависимостях см. [Архитектура](../architecture/architecture-overview.md).
