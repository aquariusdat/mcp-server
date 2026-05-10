# MoMo MCP Server — Implementation Plan

## Overview

Build a production-grade, internal MCP Server platform for MoMo using .NET 10, ASP.NET Core Minimal API, Clean Architecture, and manual CQRS. Two layers: a **Management Layer** (Admin CRUD for tools/resources/prompts) and a **Runtime Layer** (actual MCP protocol endpoint for Claude/GPT/Gemini).

---

## Solution Structure

```
mcp-server/
├── MoMo.McpServer.slnx
├── README.md
├── AGENT.md
├── docs/
│   ├── 00_context.md
│   ├── 01_architecture.md
│   ├── 02_domain_model.md
│   ├── 03_application_cqrs.md
│   ├── 04_infrastructure_storage.md
│   ├── 05_api_routes.md
│   ├── 06_runtime_layer.md
│   ├── 07_management_layer.md
│   ├── 08_decisions.md
│   └── 09_todo.md
├── data/
│   ├── tools/
│   ├── resources/
│   └── prompts/
└── src/
    ├── MoMo.McpServer.Domain/
    ├── MoMo.McpServer.Contract/
    ├── MoMo.McpServer.Application/
    ├── MoMo.McpServer.Infrastructure/
    └── MoMo.McpServer.Api/
```

---

## Project References

```
Domain         → (no references)
Contract       → Domain
Application    → Domain, Contract
Infrastructure → Domain, Application
Api            → Application, Contract, Infrastructure
```

---

## NuGet Packages

| Project | Package |
|---|---|
| Api | `ModelContextProtocol.AspNetCore` (v1.3.0) |
| Api | `Microsoft.AspNetCore.OpenApi` |

---

## Proposed Changes

### Layer 1: Domain

#### [NEW] `MoMo.McpServer.Domain.csproj`
Pure domain. No external dependencies.

#### [NEW] `Entities/BaseDefinition.cs`
- `Id` (Guid), `Code`, `Name`, `Description`, `Type`, `Category`, `Tags`, `Enabled`, `CreatedAt`, `UpdatedAt`, `Version`

#### [NEW] `Entities/McpToolDefinition.cs`
Extends `BaseDefinition`. Adds: `InputSchema` (JSON string), `OutputSchema` (JSON string), `HandlerRoute`.

#### [NEW] `Entities/McpResourceDefinition.cs`
Extends `BaseDefinition`. Adds: `Uri`, `MimeType`, `HandlerRoute`.

#### [NEW] `Entities/McpPromptDefinition.cs`
Extends `BaseDefinition`. Adds: `Template`, `Arguments` (list of prompt arg definitions).

#### [NEW] `ValueObjects/DefinitionId.cs`, `DefinitionCode.cs`, `DefinitionCategory.cs`, `DefinitionTag.cs`
Simple strongly typed value objects (record types).

---

### Layer 2: Contract

#### [NEW] `MoMo.McpServer.Contract.csproj`

#### [NEW] `Tools/CreateToolRequest.cs`, `UpdateToolRequest.cs`, `ToolResponse.cs`
#### [NEW] `Resources/CreateResourceRequest.cs`, `UpdateResourceRequest.cs`, `ResourceResponse.cs`
#### [NEW] `Prompts/CreatePromptRequest.cs`, `UpdatePromptRequest.cs`, `PromptResponse.cs`
#### [NEW] `Common/ApiResponse.cs`, `PagedResult.cs`

---

### Layer 3: Application

#### [NEW] `MoMo.McpServer.Application.csproj`

#### CQRS Abstractions
- `Abstractions/ICommand.cs`, `IQuery.cs`
- `Abstractions/ICommandHandler.cs`, `IQueryHandler.cs`
- `Abstractions/IDispatcher.cs`
- `Abstractions/Unit.cs`
- `Dispatcher/Dispatcher.cs` — DI-based dispatcher, resolves handlers from container

#### Tool Commands + Handlers
- `Tools/Commands/CreateToolCommand.cs` + `CreateToolCommandHandler.cs`
- `Tools/Commands/UpdateToolCommand.cs` + `UpdateToolCommandHandler.cs`
- `Tools/Commands/DeleteToolCommand.cs` + `DeleteToolCommandHandler.cs`
- `Tools/Commands/EnableToolCommand.cs` + `EnableToolCommandHandler.cs`
- `Tools/Commands/DisableToolCommand.cs` + `DisableToolCommandHandler.cs`

#### Tool Queries + Handlers
- `Tools/Queries/GetToolByIdQuery.cs` + `GetToolByIdQueryHandler.cs`
- `Tools/Queries/ListToolsQuery.cs` + `ListToolsQueryHandler.cs`

#### Resource Commands + Handlers (same pattern)
#### Prompt Commands + Handlers (same pattern)

#### Interfaces
- `Interfaces/IToolRepository.cs`
- `Interfaces/IResourceRepository.cs`
- `Interfaces/IPromptRepository.cs`

---

### Layer 4: Infrastructure

#### [NEW] `MoMo.McpServer.Infrastructure.csproj`

#### `Storage/JsonFileToolRepository.cs` — Implements `IToolRepository`, reads/writes JSON files under `/data/tools/`
#### `Storage/JsonFileResourceRepository.cs`
#### `Storage/JsonFilePromptRepository.cs`
#### `Storage/StorageOptions.cs` — Options pattern for base path
#### `DependencyInjection/InfrastructureServiceExtensions.cs`

---

### Layer 5: Api

#### [NEW] `MoMo.McpServer.Api.csproj`

#### `Program.cs` — Clean startup, wires everything
#### `Endpoints/Management/ToolEndpoints.cs` — `/admin/tools` CRUD
#### `Endpoints/Management/ResourceEndpoints.cs` — `/admin/resources` CRUD
#### `Endpoints/Management/PromptEndpoints.cs` — `/admin/prompts` CRUD
#### `Endpoints/Runtime/McpRuntimeEndpoints.cs` — `/mcp` SSE endpoint via `ModelContextProtocol.AspNetCore`
#### `DependencyInjection/ApplicationServiceExtensions.cs`
#### `DependencyInjection/McpServiceExtensions.cs`

---

## MCP Runtime Integration

The `ModelContextProtocol.AspNetCore` package handles the actual MCP protocol (SSE/HTTP, `initialize`, `tools/list`, `tools/call`, `resources/list`, `prompts/list`, etc).

At startup, we dynamically register tools/resources/prompts from the stored definitions. The MCP SDK uses attribute-based or programmatic registration. We'll use **programmatic registration** via `IMcpServerBuilder` so we can load from the file store.

Pattern:
```csharp
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<DynamicToolProvider>();
```

A `DynamicToolProvider` will read enabled tools from `IToolRepository` and expose them to the MCP runtime.

---

## Seed Data

Bootstrap with sample JSON files in `/data/tools/`, `/data/resources/`, `/data/prompts/`.

---

## Verification Plan

- Run `dotnet build` — expect clean build
- `GET /admin/tools` — returns list of tools from JSON files
- `POST /admin/tools` — creates a new tool JSON file
- `GET /mcp` — MCP SSE handshake succeeds (readable via curl or Claude Desktop)

---

## Open Questions

> [!NOTE]
> No blocking open questions. The plan proceeds with the following assumptions:
> - .NET 10 SDK is installed on the developer's machine
> - `ModelContextProtocol.AspNetCore` v1.3.0 will be used
> - File storage uses UTF-8 JSON per-entity files (one file per definition)
> - Data folder is `{ContentRootPath}/data/` (configurable via `appsettings.json`)
> - MCP dynamic registration: we use a custom `McpServerTool` programmatic approach to register tools from the store at startup

