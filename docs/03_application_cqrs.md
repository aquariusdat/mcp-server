# 03 — Application / CQRS

## Design Philosophy

- **No external mediator library** (no MediatR, no Wolverine)
- CQRS implemented manually using interfaces + DI-based dispatcher
- Commands = writes. Queries = reads. Never mixed.
- All handlers registered explicitly in `ApplicationServiceExtensions`

## Core Abstractions

```
ICommand<TResult>                    — marker for all commands
IQuery<TResult>                      — marker for all queries
ICommandHandler<TCommand, TResult>   — implement for each command
IQueryHandler<TQuery, TResult>       — implement for each query
IDispatcher                          — send commands/queries
Unit                                 — void return type for commands
```

## Dispatcher

`Dispatcher` (in `Application/Dispatcher/`) resolves handlers via `IServiceProvider`. Uses reflection to call `HandleAsync`. Simple, no external dependencies.

**Endpoints only interact with IDispatcher — never with handlers directly.**

## Tool Commands

| Command | Return | Action |
|---|---|---|
| `CreateToolCommand` | `ToolResponse` | Create a new tool |
| `UpdateToolCommand` | `ToolResponse?` | Update existing tool |
| `DeleteToolCommand` | `bool` | Delete tool |
| `EnableToolCommand` | `bool` | Enable tool |
| `DisableToolCommand` | `bool` | Disable tool |

## Tool Queries

| Query | Return | Action |
|---|---|---|
| `GetToolByIdQuery` | `ToolResponse?` | Get tool by Guid |
| `ListToolsQuery` | `IReadOnlyList<ToolResponse>` | List all (optional enabled filter) |

*(Same pattern for Resources and Prompts)*

## Runtime Capability Providers

The Application layer also provides the boundary for the MCP Runtime via Capability Providers (`IToolCapabilityProvider`, etc.). These wrap the repositories to only expose **enabled** definitions for discovery.

## Handler Registration

All handlers and capability providers are registered in `ApplicationServiceExtensions.AddApplicationServices()`.

## Mapping

`DefinitionMapper` (in `Application/Mapping/`) contains extension methods to map domain entities to response DTOs.
