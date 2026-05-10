# 01 — Architecture

## Layer Structure

```
┌──────────────────────────────────────────────────────────┐
│  Api                                                     │
│  (Minimal API endpoints, MCP runtime, DI wiring)         │
├──────────────────────────────────────────────────────────┤
│  Infrastructure                                          │
│  (File-based JSON storage, NoOpToolRegistry)             │
├──────────────────────────────────────────────────────────┤
│  Application                                             │
│  (CQRS handlers, capability providers, executors)        │
├──────────────────────────────────────────────────────────┤
│  Contract                        Domain                  │
│  (DTOs, ApiResponse)             (Entities, rules)       │
└──────────────────────────────────────────────────────────┘
```

## Dependency Direction

- **Domain** depends on nothing.
- **Contract** depends on nothing. (Pure DTOs).
- **Application** depends on Domain + Contract.
- **Infrastructure** depends on Domain + Application.
- **Api** depends on Application + Contract + Infrastructure.

*Dependencies flow inward to Application/Domain. Contract and Domain are independent siblings.*

## Architectural Seams

### 1. Management Seam (CQRS)
API endpoints use `IDispatcher` to send commands/queries to the Application layer.
Application layer uses `IToolRepository` to read/write from Infrastructure.

### 2. Runtime Capability Seam
MCP Runtime Providers (`Api/Runtime/`) use `IToolCapabilityProvider` to read enabled tools.
They **do not** have access to the underlying `IToolRepository`.

### 3. Execution Seam
When an MCP client invokes a tool, the runtime asks `IToolRegistry` for an `IToolExecutor` mapped to the tool's `HandlerRoute`. The executor handles the actual work and returns a `ToolExecutionResult`.
