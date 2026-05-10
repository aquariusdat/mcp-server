# AGENT.md — MoMo MCP Server Working Guide

This file is the persistent working guide for AI agents working on this codebase.
Before generating any code changes, read this file first and respect the rules below.

---

## Project Purpose

Internal MCP Server platform for MoMo. Allows any team (starting with CRM) to:
1. Register MCP tool/resource/prompt definitions via the Management Layer
2. Expose them to AI clients (Claude Desktop, GPT, Gemini) via the MCP Runtime Layer

---

## Architecture Summary

Five layers, dependency flows **inward only**:

```
Domain ← Contract ← Application ← Infrastructure
                              ↑
                             Api
```

- **Domain**: Entities and business rules. No external dependencies.
- **Contract**: DTOs and request/response models. References Domain.
- **Application**: CQRS handlers, repository interfaces, dispatcher. References Domain + Contract.
- **Infrastructure**: File-based storage implementations. References Domain + Application.
- **Api**: Minimal API endpoints + MCP runtime wiring. References Application + Contract + Infrastructure.

---

## Layer Responsibilities

| Layer | Must contain | Must NOT contain |
|---|---|---|
| Domain | Entities, value objects, domain rules | Infrastructure, HTTP, DI |
| Contract | DTOs, requests, responses | Business logic |
| Application | Commands, queries, handlers, interfaces | HTTP concerns, file I/O |
| Infrastructure | File storage, serialization | Business logic |
| Api | Endpoints, DI wiring, MCP SDK setup | Business logic, file I/O |

---

## Naming Conventions

- Commands: `{Verb}{Entity}Command` — e.g., `CreateToolCommand`, `EnableResourceCommand`
- Queries: `{Get|List}{Entity}Query` — e.g., `GetToolByIdQuery`, `ListPromptsQuery`
- Handlers: `{Command/Query}Handler` — e.g., `CreateToolCommandHandler`
- Repositories: `I{Entity}Repository` — e.g., `IToolRepository`
- Response DTOs: `{Entity}Response` — e.g., `ToolResponse`, `ResourceResponse`
- Request DTOs: `Create/Update{Entity}Request` — e.g., `CreateToolRequest`
- Endpoints: `{Entity}Endpoints` with `Map{Entity}Endpoints()` extension

---

## CQRS Rules

- Commands = writes. Always return a value (never void — use `bool` or the response DTO).
- Queries = reads. Never modify state.
- All handlers implement `ICommandHandler<TCommand, TResult>` or `IQueryHandler<TQuery, TResult>`.
- The `IDispatcher` is the only public CQRS entry point. Endpoints only call `dispatcher.SendCommandAsync()` or `dispatcher.SendQueryAsync()`.
- Do NOT call handlers directly from endpoints.
- Register all handlers in `ApplicationServiceExtensions.AddApplicationServices()`.

---

## Management Layer Rules

- Routes under `/admin/{tools|resources|prompts}`
- Full CRUD: GET (list), GET (by id), POST (create), PUT (update), DELETE
- State change: PATCH `/{id}/enable` and `/{id}/disable`
- All responses wrapped in `ApiResponse<T>` envelope
- Endpoints are THIN — delegate everything to IDispatcher

---

## Runtime Layer Rules

- MCP protocol handled by `ModelContextProtocol.AspNetCore` SDK
- Runtime endpoint: `/mcp` (HTTP/SSE transport)
- Provider classes in `Api/Runtime/` decorated with `[McpServerToolType]`
- Methods decorated with `[McpServerTool(Name = "...")]` and `[Description("...")]`
- Providers are thin — only read from repos and format for MCP
- No business logic in providers

---

## Storage Rules

- One JSON file per entity: `{BasePath}/{entity-type}/{id}.json`
- Default BasePath: `{ContentRootPath}/data` (configured via `appsettings.json > Storage:BasePath`)
- Storage implementations in `Infrastructure/Storage/`
- All repositories implement interfaces in `Application/Interfaces/`
- Application layer NEVER imports Infrastructure types directly
- Graceful error handling: corrupt/missing files return null, not exceptions

---

## MCP Schema Rules

- Tool `InputSchema` and `OutputSchema` must be valid JSON Schema strings (draft-07)
- Tool `Code` must be unique and machine-readable (snake_case preferred)
- Resource `Uri` must be a stable, unique URI (use `momo://` scheme)
- Prompt `Template` may contain `{{argumentName}}` placeholders
- Prompt `Arguments` must list all placeholders with `Required` flags

---

## What Must Not Be Changed Without Updating Docs

1. Layer dependency direction
2. CQRS handler registration in `ApplicationServiceExtensions`
3. Storage path convention (`{BasePath}/{type}/{id}.json`)
4. MCP runtime provider class/method naming conventions
5. `ApiResponse<T>` envelope contract
6. Route prefix convention (`/admin/` for management, `/mcp` for runtime)

---

## Docs Source of Truth

| Doc | Contents |
|---|---|
| `docs/01_architecture.md` | Layer structure and dependency rules |
| `docs/03_application_cqrs.md` | CQRS design and handler registration |
| `docs/04_infrastructure_storage.md` | Storage conventions |
| `docs/05_api_routes.md` | All routes and HTTP contracts |
| `docs/06_runtime_layer.md` | MCP SDK integration |
| `docs/08_decisions.md` | Architecture decision records |

Before introducing ANY change, read the relevant doc and update it after.
