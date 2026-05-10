# 01 — Architecture

## Layer Structure

```
┌──────────────────────────────────────────────────────────┐
│  Api                                                     │
│  (Minimal API endpoints, MCP runtime, DI wiring)         │
├──────────────────────────────────────────────────────────┤
│  Infrastructure                                          │
│  (File-based JSON storage, serialization)                │
├──────────────────────────────────────────────────────────┤
│  Application                                             │
│  (CQRS commands, queries, handlers, interfaces)          │
├──────────────────────────────────────────────────────────┤
│  Contract                                                │
│  (DTOs, request/response models)                         │
├──────────────────────────────────────────────────────────┤
│  Domain                                                  │
│  (Entities, business rules — no dependencies)            │
└──────────────────────────────────────────────────────────┘
```

## Dependency Direction

```
Domain ← Contract ← Application ← Infrastructure
                              ↑
                             Api
```

- Dependencies flow **inward** only.
- **Domain** has zero dependencies.
- **Application** depends on Domain + Contract (not Infrastructure).
- **Infrastructure** implements Application interfaces.
- **Api** references Application + Contract + Infrastructure for DI wiring only.

## Project References

| Project | References |
|---|---|
| Domain | (none) |
| Contract | Domain |
| Application | Domain, Contract |
| Infrastructure | Domain, Application |
| Api | Application, Contract, Infrastructure |

## Two Runtime Concerns

| Concern | Routes | Description |
|---|---|---|
| Management Layer | `/admin/...` | Admin REST API for CRUD |
| Runtime Layer | `/mcp` | MCP protocol endpoint for AI clients |
