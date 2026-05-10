# MoMo MCP Server

An internal **Model Context Protocol (MCP) Server** platform for MoMo, built with:

- **.NET 10** + **ASP.NET Core Minimal API**
- **Clean Architecture** (Domain → Contract → Application → Infrastructure → Api)
- **Manual CQRS** (no external mediator library)
- **Local file-based storage** for MVP (easy to replace with DB later)
- **Official MCP C# SDK** (`ModelContextProtocol.AspNetCore` v1.3.0)

---

## Quick Start

```bash
cd src/MoMo.McpServer.Api
dotnet run
```

The server starts at `https://localhost:7xxx` / `http://localhost:5xxx`.

---

## Two Layers

### Management Layer (Admin)

REST API for managing MCP definitions:

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/admin/tools` | List all tools |
| GET | `/admin/tools/{id}` | Get tool by ID |
| POST | `/admin/tools` | Create tool |
| PUT | `/admin/tools/{id}` | Update tool |
| DELETE | `/admin/tools/{id}` | Delete tool |
| PATCH | `/admin/tools/{id}/enable` | Enable tool |
| PATCH | `/admin/tools/{id}/disable` | Disable tool |

Same pattern for `/admin/resources` and `/admin/prompts`.

### Runtime Layer (MCP Protocol)

MCP-compliant endpoint for Claude Desktop, GPT, Gemini:

```
/mcp          ← MCP SSE endpoint
```

Configure in Claude Desktop `claude_desktop_config.json`:
```json
{
  "mcpServers": {
    "momo": {
      "url": "http://localhost:5000/mcp"
    }
  }
}
```

---

## Solution Structure

```
src/
├── MoMo.McpServer.Domain/          Pure entities, no dependencies
├── MoMo.McpServer.Contract/        DTOs, request/response models
├── MoMo.McpServer.Application/     CQRS commands, queries, handlers
├── MoMo.McpServer.Infrastructure/  File-based JSON storage
└── MoMo.McpServer.Api/             Minimal API endpoints + MCP runtime
    └── data/
        ├── tools/                  Tool JSON files (one per tool)
        ├── resources/              Resource JSON files
        └── prompts/                Prompt JSON files
```

---

## Storage

Data lives in `src/MoMo.McpServer.Api/data/` by default. Each definition is stored as a single JSON file named `{id}.json`. Override the path in `appsettings.json`:

```json
{
  "Storage": {
    "BasePath": "C:/custom/path/to/data"
  }
}
```

---

## Architecture Docs

See the [`docs/`](./docs/) folder for full architecture documentation:

- [`docs/01_architecture.md`](./docs/01_architecture.md) — Layer overview
- [`docs/03_application_cqrs.md`](./docs/03_application_cqrs.md) — CQRS design
- [`docs/05_api_routes.md`](./docs/05_api_routes.md) — All routes
- [`docs/06_runtime_layer.md`](./docs/06_runtime_layer.md) — MCP runtime
- [`docs/08_decisions.md`](./docs/08_decisions.md) — Architecture decisions

---

## OpenAPI

Available at `/openapi/v1.json` in development mode.

---

## Future Roadmap

- [ ] Replace file storage with PostgreSQL/Redis
- [ ] Add authentication (API key or JWT)
- [ ] Add real tool handler dispatch (HTTP, gRPC, or internal)
- [ ] Add per-team tool namespacing
- [ ] Add metrics and observability
