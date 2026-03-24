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

### Через Docker Compose

Для локального запуска контейнеров приложения, PostgreSQL и стека мониторинга используйте:

```powershell
docker compose -f docker-compose.Development.yml up --build -d
```

### Что запускается

AppHost автоматически поднимает:

| Ресурс | Описание |
|---|---|
| **PostgreSQL** | Контейнер с БД, persistent volume |
| **PgAdmin** | Веб-интерфейс для администрирования БД |
| **HomeAccounting** | ASP.NET Core бэкенд |

Миграции БД применяются автоматически при старте бэкенда.

При запуске через `docker-compose.Development.yml` дополнительно поднимаются:

| Ресурс | Описание |
|---|---|
| **Grafana Alloy** | Приём OTLP и маршрутизация телеметрии |
| **Prometheus** | Сбор и хранение метрик |
| **Loki** | Хранение логов |
| **Tempo** | Хранение трейсов |
| **Grafana** | Дашборды и исследование телеметрии |

## Grafana provisioning (Development)

Базовая конфигурация Grafana хранится в репозитории:

- Data sources: `ops/monitoring/grafana/datasources/datasources.yaml`
- Dashboard provisioning: `ops/monitoring/grafana/dashboards/provisioning.yaml`
- Managed dashboards (JSON): `ops/monitoring/grafana/dashboards/*.json`
- Alerting provisioning: `ops/monitoring/grafana/alerting/*.yaml`

### Managed vs ad-hoc dashboards

- **Managed dashboards**: файлы в `ops/monitoring/grafana/dashboards/*.json`. Они являются source of truth и автоматически подгружаются при старте Grafana.
- **Ad-hoc dashboards**: создаются в UI Grafana для быстрых экспериментов и хранятся во внутренней БД Grafana (volume `homeaccounting-dev-grafana-data`).
- Изменения provisioned dashboards через UI могут быть перезаписаны при следующем обновлении provisioning.

### Как добавить новый managed dashboard

1. Создайте/обновите дашборд в UI Grafana.
2. Экспортируйте dashboard в JSON.
3. Перед сохранением в репозиторий убедитесь, что:
   - `id` равен `null`;
   - `uid` стабильный и уникальный.
4. Положите JSON в `ops/monitoring/grafana/dashboards/`.
5. Перезапустите Grafana или дождитесь автообновления (`updateIntervalSeconds`).

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
