# AGENT.md — MoMo MCP Server Working Guide

This file is the persistent working guide for AI agents working on this codebase.
Before generating any code changes, read this file first and respect the rules below.

---

## Project Purpose

Internal MCP Server platform for MoMo. Allows any team (starting with CRM) to:
1. Register MCP tool/resource/prompt definitions via the Management Layer
2. Expose them to AI clients (Claude Desktop, GPT, Gemini) via the MCP Runtime Layer
3. (Future) Dispatch MCP tool calls to concrete executors

---

## Architecture Summary

Five layers, dependency flows **inward only**:

```
Domain ← Contract   Application ← Infrastructure
              ↖       ↑
                 Api
```

*Note: Contract and Domain have NO dependencies on each other. Application depends on both.*

- **Domain**: Entities and business rules (`BaseDefinition`, `McpToolDefinition`).
- **Contract**: Pure DTOs and request/response models (`ToolResponse`, `ApiResponse`).
- **Application**: CQRS handlers, interfaces, and runtime abstractions (`IToolCapabilityProvider`, `IToolExecutor`).
- **Infrastructure**: Implementations for storage and execution (`JsonFileToolRepository`, `NoOpToolRegistry`).
- **Api**: Minimal API endpoints + MCP runtime providers.

---

## Runtime vs Definition Separation (CRITICAL)

Do not mix definition metadata with runtime execution.
- **Definitions**: Managed via `IToolRepository`. Mapped via `/admin` endpoints.
- **Capabilities**: Read via `IToolCapabilityProvider`. Tells the MCP runtime what's available.
- **Execution**: Handled by `IToolRegistry.Resolve(code) -> IToolExecutor`.

Providers in `Api/Runtime/` (e.g., `McpToolProvider`) must NEVER access repositories directly. They only use capability providers and registries.

---

## Naming Conventions

- Commands: `{Verb}{Entity}Command` — e.g., `CreateToolCommand`
- Queries: `{Get|List}{Entity}Query` — e.g., `ListToolsQuery`
- Handlers: `{Command/Query}Handler` — e.g., `CreateToolCommandHandler`
- Repositories: `I{Entity}Repository` — e.g., `IToolRepository`
- Capability Providers: `I{Entity}CapabilityProvider` — e.g., `IToolCapabilityProvider`
- Executors: `I{Entity}Executor` — e.g., `IToolExecutor`

---

## CQRS Rules

- Commands = writes. Always return a value (never void — use `bool` or the response DTO).
- Queries = reads. Never modify state.
- All handlers implement `ICommandHandler<TCommand, TResult>` or `IQueryHandler<TQuery, TResult>`.
- The `IDispatcher` is the only public CQRS entry point. Endpoints only call `dispatcher.SendCommandAsync()` or `dispatcher.SendQueryAsync()`.

---

## Schema Rules

- `InputSchema` and `OutputSchema` are represented as `JsonNode`, NOT strings.
- This guarantees valid JSON structure throughout the stack and serializes cleanly.

---

## Versioning Strategy

- `Code` = Stable machine-readable identity (e.g. `"crm.get_customer"`). This does not change.
- `Version` = Schema/content iteration (integer). Incremented on meaningful updates.
- Future MCP clients may discover tools using `Code:Version` syntax.

---

## What Must Not Be Changed Without Updating Docs

1. Layer dependency direction (especially keeping Contract clean).
2. CQRS handler registration in `ApplicationServiceExtensions`.
3. The definition/execution boundary (`IToolRegistry` + `IToolExecutor`).
4. Schema modeling (`JsonNode`).
5. Route prefix convention (`/admin/` for management, `/mcp` for runtime).

---

## CRM JSM Mock Setup (Demo / Planning Phase)

To facilitate realistic AI-assisted Jira planning demos without calling real Atlassian APIs, a mock environment has been implemented strictly within the **Infrastructure** layer:

- **Mock Database**: `CrmMockDatabase` stores static facts (Team, Skills, Workloads).
- **Fact Executors**: Read-only executors that return hard facts (e.g. `GetTeamMembersExecutor`).
- **Reasoning Executors**: Provide deterministic simulated logic (e.g. `DraftTaskBreakdownExecutor` uses templates, `BulkCreateTasksExecutor` generates fake `CRM-XXXX` keys).
- **Registry**: `DictionaryToolRegistry` maps `momo.crm.*` routes to these executors.

**Important Rule**: Claude MUST reason ON TOP OF facts. The tools provide the facts (who is in the team, what is the capacity), and Claude makes suggestions. Some tools provide "Mock Reasoning" (`suggest_assignee`) just to enforce a deterministic outcome for the demo.
