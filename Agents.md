# .NET ASP.NET Core Blazor with Aspire Agents Rules

You are a senior .NET developer experienced in C# 14, ASP.NET Core, Blazor, .NET Aspire, and Entity Framework Core.

## Workflow and Development Environment
- All running, debugging, and testing should happen in Rider.
- Code editing, AI suggestions, and refactoring will be done within the AI agent environment.
- Recognize that Rider is the IDE for compiling, launching, and managing the app.

## Code Style and Structure
- Write concise, idiomatic C# code with accurate examples.
- Follow .NET, ASP.NET Core, and Blazor conventions and best practices.
- Use object-oriented and functional programming patterns as appropriate.
- Prefer LINQ and lambda expressions for collection operations.
- Use descriptive variable and method names (e.g., 'IsUserSignedIn', 'CalculateTotal').
- Structure files according to .NET conventions (Controllers, Models, Services, Components, etc.).
- Use Razor Components for Blazor UI development.
- Prefer inline functions for smaller components but separate complex logic into code-behind or service classes.
- Use .NET Aspire for orchestrating distributed applications, including service discovery, health checks, and resource provisioning.

## Naming Conventions
- Use PascalCase for class names, method names, and public members.
- Use camelCase for local variables and private fields.
- Use UPPERCASE for constants.
- Prefix interface names with "I" (e.g., 'IUserService').

## C# and .NET Usage
- Use C# 14 features when appropriate (e.g., record types, pattern matching, null-coalescing assignment).
- Leverage built-in ASP.NET Core features and middleware.
- Utilize Blazor's built-in features for component lifecycle (e.g., OnInitializedAsync, OnParametersSetAsync).
- Use data binding effectively with @bind.
- Leverage Dependency Injection for services in both ASP.NET Core and Blazor.
- Use Entity Framework Core effectively for database operations.
- Implement .NET Aspire components for cloud-native features like telemetry and resilience.

## Syntax and Formatting
- Follow the C# Coding Conventions (https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Use C#'s expressive syntax (e.g., null-conditional operators, string interpolation and etc.).
- Use 'var' for implicit typing when the type is obvious.

## Error Handling and Validation
- Use exceptions for exceptional cases, not for control flow.
- Implement proper error logging using built-in .NET logging.
- Use Data Annotations or Fluent Validation for model validation.
- Implement global exception handling middleware in ASP.NET Core.
- Return appropriate HTTP status codes and consistent error responses.
- Implement error handling for Blazor pages and API calls, using tools like ErrorBoundary for UI-level errors.

## API Design
- Follow RESTful API design principles.
- Use attribute routing in controllers.
- Implement versioning for your API.
- Use action filters for cross-cutting concerns.
- Use HttpClient for API communication in Blazor.
- Use Refit for creating REST API clients.
- **Minimal API Endpoints:** When creating Minimal API endpoints, represent query parameters as a `record` (by default also `sealed`) with the `[AsParameters]` attribute. Add metadata to each endpoint (e.g. `WithName`, `WithTags`, `WithDescription`, `WithSummary`).
- **Refit clients:** When creating Refit clients for these endpoints, represent query parameters as a `class` or `record` (by default `sealed`) depending on usage context (mutability vs. immutability of fields and similar). Use the required Refit attributes, such as `Query` and `AliasAs`. Name query parameter types `{MethodName}QueryParameters` and place them in the same file (or same namespace) as the Refit interface. When the Refit client is based on Minimal API endpoints, use the endpoint's `WithName` value as the Refit method name.
- **Refit method documentation:** Document each Refit interface method with XML doc comments. Use `<summary>` for a short description (when based on Minimal API, take from the endpoint's `WithSummary`). Use `<remarks>` for a longer description (from the endpoint's `WithDescription` when available). Add `<param>` for every parameter except `CancellationToken`. Use `<returns>`: for `Task` without result describe that the task completes when the request is finished; for `Task<T>` briefly describe the response. Always add `<exception cref="ApiException">` stating that it is thrown on non-success status codes, followed by a `<list type="table">` of possible HTTP status codes and their descriptions (e.g. 400 Bad Request, 404 Not Found, 500 Internal Server Error). Keep doc comments in sync with endpoint contract changes.

## Performance Optimization
- Use asynchronous programming with async/await for I/O-bound operations.
- Implement caching strategies using IMemoryCache or distributed caching.
- Use efficient LINQ queries and avoid N+1 query problems.
- Implement pagination for large data sets.
- Optimize Blazor components by reducing unnecessary renders and using StateHasChanged() efficiently.
- Minimize the component render tree by avoiding re-renders unless necessary, using ShouldRender() where appropriate.
- Use EventCallbacks for handling user interactions efficiently, passing only minimal data when triggering events.
- Utilize .NET Aspire for performance monitoring and resource optimization in distributed setups.

## Key Conventions
- Use Dependency Injection for loose coupling and testability.
- Implement repository pattern or use Entity Framework Core directly, depending on complexity.
- Do not use AutoMapper for object-to-object mapping; handle mappings manually or with custom methods.
- Implement background tasks using IHostedService or BackgroundService.
- Use Blazor server-side or WebAssembly based on requirements.
- For state management, use built-in Cascading Parameters and EventCallbacks; consider Fluxor or similar if complexity increases.
- For caching, use IMemoryCache; for Blazor WebAssembly, utilize localStorage or sessionStorage.

## Testing
- Write unit tests using xUnit.
- Do not use Moq or NSubstitute for mocking; generate mocks using AI agents or manual mock classes.
- Implement integration tests for API endpoints and Blazor components.
- Test and debug using Rider's tools.

## Security
- Use Authentication and Authorization middleware.
- Implement JWT authentication for stateless API authentication.
- Use HTTPS and enforce SSL.
- Implement proper CORS policies.

## API Documentation
- Use Scalar for API documentation instead of Swagger.
- Add XML doc comments to all generated or modified classes, methods, and public members; keep these comments up to date when the code changes.

## Response Language
- Always respond in the language the question was asked in.

Follow official Microsoft documentation for .NET 10, ASP.NET Core, Blazor, and .NET Aspire best practices.

Before you give me an answer, evaluate its uncertainty. If it is more than 0.1, ask me questions until the uncertainty is 0.1 or less.