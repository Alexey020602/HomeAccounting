# ADR-0002: Вертикальные срезы

## Статус

Accepted

## Контекст

При организации кода внутри модулей (bounded context'ов) нужно было выбрать способ группировки: по техническим слоям (Controllers, Services, Repositories) или по бизнес-сценариям.

## Варианты

1. **По слоям (layered)** — классическая структура: Controllers/, Services/, Repositories/, Models/. Код одного use case размазан по нескольким папкам.
2. **По вертикальным срезам (vertical slices)** — один use case — одна папка, содержащая все связанные файлы: endpoint, запрос, ответ, обработчик.
3. **Гибрид** — слои + feature folders. Сложнее в навигации.

## Решение

Выбраны вертикальные срезы. Каждый use case — отдельная папка внутри модуля:

```
HomeAccounting/
  Budgets/
    CreateBudget/
      CreateBudgetEndpoint.cs
      CreateBudgetRequest.cs
    AddManualSpending/
      AddManualSpendingEndpoint.cs
      ...
    GetReceipts/
      GetReceiptsEndpoint.cs
      GetReceiptsRequest.cs
      ...
```

Используются Minimal API endpoints. 

## Последствия

**Положительные:**
- Высокая cohesion: все файлы одного сценария в одном месте.
- Легко находить и изменять код конкретного use case.
- Минимизация конфликтов при параллельной разработке (каждый сценарий — изолированная папка).
- Хорошо сочетается с Minimal API (один endpoint — один файл).

**Отрицательные:**
- Общая логика (shared между сценариями) может дублироваться или требовать выноса в `Common`/`Data`.
