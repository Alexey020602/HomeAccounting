# Аутентификация и авторизация

Подробнее о причинах выбора: [ADR-0003: JWT + Refresh Token](../decisions/0003-jwt-refresh-token.md)

## Обзор

Аутентификация построена на JWT access token + refresh token с ротацией. Используется ASP.NET Core Identity для хранения пользователей и собственная реализация управления токенами.

## Конфигурация JWT

Секция `JwtTokenSettings` в `appsettings.json`:

| Параметр | Значение по умолчанию | Описание |
|---|---|---|
| `Issuer` | `"ValidIssuer"` | Издатель токена |
| `Audience` | `"ValidAudience"` | Аудитория токена |
| `Key` | (секрет) | Ключ подписи (HMAC-SHA256) |
| `AccessTokenExpireMinutes` | 1 | Время жизни access token |
| `RefreshTokenExpireDays` | 1 | Время жизни refresh token |

Валидация: `ValidateLifetime = true`, `ClockSkew = 30 секунд`. Проверки Issuer/Audience отключены в текущей конфигурации.

## Жизненный цикл токенов

### Вход (Login)

1. Пользователь отправляет логин и пароль.
2. Сервер генерирует JWT access token (HMAC-SHA256, claims: `Jti` + user claims).
3. Генерируется refresh token: 32 случайных байта → Base64.
4. Refresh token хешируется (SHA256) и сохраняется в БД с `SessionId`, `JwtId`, `UserId`, `ExpiresAt`.
5. Клиенту возвращаются access token и refresh token (в открытом виде).

### Обновление (Refresh)

1. Клиент отправляет текущий access token (может быть истёкшим) и refresh token.
2. Сервер извлекает `Jti` из access token.
3. Ищет refresh token в БД по хешу.
4. Проверяет: не истёк, не использован, `JwtId` совпадает.
5. **Детекция повторного использования:** если refresh token уже был использован — удаляются все токены этой сессии (защита от кражи).
6. Старый refresh token помечается как использованный (`UsedAt`).
7. Генерируются новый access token и новый refresh token (с тем же `SessionId`).

### Выход (Logout)

- `POST /api/logout` — отзыв refresh token текущей сессии (`RevokedAt`).
- `POST /api/logout/all` — отзыв всех refresh token пользователя.
- На клиенте: удаление данных из `localStorage`.

## Хранение на клиенте

Токены хранятся в `localStorage` браузера через `AuthenticationStorage`:

- Ключ: `"Authorization"`
- Значение: JSON с полями `AccessToken`, `RefreshToken`, `User`, `ExpiresAt`, `RefreshTokenExpiresAt`

## HTTP-перехватчик (AuthenticationHandler)

`AuthenticationHandler` — `DelegatingHandler`, который автоматически:

1. Добавляет `Authorization: Bearer {token}` к запросам.
2. Если access token истёк — вызывает `ITokenService.GetFreshAccessToken()` для автоматического refresh.
3. При получении 401 — пробует обновить токен повторно.
4. Если refresh не удался — вызывает `ILogoutService.Logout()` и возвращает 401.

## Очистка токенов

`TokensCleanupWorker` — фоновый сервис, который удаляет старые refresh-токены:

- Интервал: каждые 7 дней (`TokensCleanup:DayInterval`)
- Удаляет токены, истёкшие более 30 дней назад
- Пакетами по 50 000 записей

## Авторизация endpoints

Бэкенд endpoints сгруппированы в `app.MapGroup("api").RequireAuthorization()`. Endpoints авторизации (login, register, refresh, logout) — анонимные.

Для ресурсной авторизации в модуле Budgets используется `BudgetRequirements(BudgetPermissions.Read/Edit/Delete)` через `IAuthorizationService`.
