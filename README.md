# Horizon — Vacation Tracker

Система учёта и согласования отпусков. Учебный проект.

Диаграммы (доступ только на просмотр):
1. Основные сущности — https://mermaid.ai/d/67bea33b-a251-4b6b-bafe-2f137653a8f9
2. Сущности, связанные с бизнес-процессом — https://mermaid.ai/d/abb32b9b-9a94-4ba9-b615-bd3db3540eb9
3. Схема бизнес-процесса — https://mermaid.ai/d/b6ec8337-5303-413c-9ae2-e32e6ae64ba4

---

## Технологический стек

| Слой | Технология |
|------|-----------|
| Backend | .NET 10, ASP.NET Core, EF Core 10, Npgsql |
| CQRS / Messaging | MediatR |
| Валидация | FluentValidation |
| База данных | PostgreSQL |
| Frontend | React 18, TypeScript, Vite, Redux Toolkit (RTK Query), MUI |

---

## MVP-критерии

| Критерий | Баллы | Реализация |
|----------|-------|-----------|
| База данных | 10 | PostgreSQL + EF Core миграции |
| Асинхронность | 5 | Весь I/O — `async/await` (EF Core, HTTP-handlers) |
| Примитивы синхронизации | 15 | `SemaphoreSlim` в `RequestNumberGenerator` |
| Шаблоны проектирования (≥ 3) | 60 | 6 паттернов — см. ниже |
| Объём работы | 30 | — |
| **Итого** | **120** | |

---

## Паттерны проектирования

### 1. Command (Команда) — MediatR CQRS

**Где:** `Horizon.Server/Modules/ApprovalWorkflow/Application/Commands/`

**Суть:** Каждая бизнес-операция (создать заявку, согласовать, отклонить, вернуть) оформлена как отдельный объект-команда (`CreateRequestCommand`, `ApproveRequestCommand`, `RejectRequestCommand`, `SendBackRequestCommand`). Отправитель ничего не знает об исполнителе — только вызывает `IMediator.Send(command)`.

```csharp
// Controller — только отправляет команду:
var id = await _mediator.Send(new CreateRequestCommand(...));

// Handler — исполняет команду, изолированно:
public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
{
    public async Task<int> Handle(CreateRequestCommand command, CancellationToken ct) { ... }
}
```

---

### 2. Observer / Domain Events (Наблюдатель / Доменные события)

**Где:**
- `Horizon.Server/Modules/Shared/Domain/Common/BaseEntity.cs` — хранит список событий
- `Horizon.Server/Modules/Shared/Infrastructure/Persistence/UnitOfWork.cs` — диспатчит события
- `Horizon.Server/Modules/ApprovalWorkflow/Domain/Events/` — конкретные события
- `Horizon.Server/Modules/ApprovalWorkflow/Application/EventHandlers/HandleLeaveRequestedEvent.cs` — обработчик

**Суть:** Доменные сущности поднимают события (`RequestSubmittedEvent`, `RequestApprovedEvent`, `RequestRejectedEvent`, `RequestSentBackEvent`). После `SaveChangesAsync` `UnitOfWork` автоматически диспатчит все накопленные события через `IPublisher` MediatR. Обработчики реагируют независимо — без прямой связи с источником.

```csharp
// UnitOfWork собирает и диспатчит события:
var domainEvents = _context.ChangeTracker
    .Entries<BaseEntity>()
    .SelectMany(e => e.Entity.DomainEvents)
    .ToList();
await _context.SaveChangesAsync(cancellationToken);
foreach (var domainEvent in domainEvents)
    await _publisher.Publish(domainEvent, cancellationToken);

// Независимый обработчик:
public class HandleLeaveRequestedEvent : INotificationHandler<RequestSubmittedEvent>
{
    public Task Handle(RequestSubmittedEvent notification, CancellationToken ct) { ... }
}
```

---

### 3. State (Состояние)

**Где:** `Horizon.Server/Modules/ApprovalWorkflow/Domain/Entities/Request.cs`

**Суть:** Сущность `Request` сама управляет переходами между статусами через инкапсулированные методы. Внешний код не меняет статус напрямую — он вызывает метод, который применяет правила перехода и поднимает нужное доменное событие.

```csharp
public class Request : BaseEntity
{
    public void Submit()  => AddDomainEvent(new RequestSubmittedEvent(...));
    public void Approve(int newStatusId, int? nextStage, bool isFinal) { StatusId = newStatusId; ... }
    public void Reject(int rejectedStatusId, string? reason)           { StatusId = rejectedStatusId; ... }
    public void SendBack(int sentBackStatusId, string? comment)        { StatusId = sentBackStatusId; ... }
}
```

---

### 4. Repository (Репозиторий)

**Где:** `Horizon.Server/Modules/**/Infrastructure/Persistence/Repositories/`

**Суть:** Каждый агрегат имеет свой репозиторий, скрывающий детали доступа к данным. Прикладной код работает с интерфейсом (`IRequestRepository`), а не с EF Core напрямую. Это упрощает тестирование и замену ORM.

```csharp
public interface IRequestRepository
{
    Task<Request?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Request>> GetAllByEmployeeIdAsync(int employeeId);
    Task AddAsync(Request request);
}
```

---

### 5. Unit of Work (Единица работы)

**Где:**
- `Horizon.Server/Modules/Shared/Domain/Abstractions/IUnitOfWork.cs`
- `Horizon.Server/Modules/Shared/Infrastructure/Persistence/UnitOfWork.cs`

**Суть:** Координирует несколько репозиториев в рамках одной бизнес-операции. Гарантирует, что либо все изменения сохраняются атомарно (`ExecuteInTransactionAsync`), либо откатываются при ошибке. Также отвечает за диспатч доменных событий.

```csharp
await _unitOfWork.ExecuteInTransactionAsync(async () =>
{
    await _requestRepository.AddAsync(request);
    await _unitOfWork.SaveChangesAsync(); // сохранение + диспатч событий
});
```

---

### 6. Pipeline / Decorator (Конвейер / Декоратор валидации)

**Где:**
- `Horizon.Server/Modules/Shared/Application/Behaviours/ValidationPipelineBehaviour.cs`
- `Horizon.Server/API/Filters/ValidationFilter.cs`

**Суть:** MediatR Pipeline Behavior оборачивает каждую команду в слой валидации FluentValidation до вызова handler. Decorator добавляет поведение (проверку) без изменения исходного объекта — классический Decorator/Pipeline.

```csharp
public class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        // Validate BEFORE the real handler runs:
        var failures = _validators.SelectMany(v => v.Validate(request).Errors).ToList();
        if (failures.Any()) throw new ValidationException(failures);
        return await next(); // delegate to the actual handler
    }
}
```

---

## Запуск проекта

### Backend

```bash
cd Horizon.Server
dotnet run
# Swagger: https://localhost:7158/swagger
```

### Frontend

```bash
cd horizon.client
npm install
npm run dev -- --host 0.0.0.0
# UI: http://localhost:5173
```

### Требования

- .NET 10 SDK
- Node.js 18+
- PostgreSQL (строка подключения в `appsettings.json` → `ConnectionStrings:PostgresConnection`)
