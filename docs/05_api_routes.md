# 05 — API Routes

## Management Layer

Base prefix: `/admin`

### Tools

| Method | Path | Description |
|---|---|---|
| GET | `/admin/tools` | List all tools |
| GET | `/admin/tools/{id}` | Get tool by GUID |
| POST | `/admin/tools` | Create tool |
| PUT | `/admin/tools/{id}` | Update tool |
| DELETE | `/admin/tools/{id}` | Delete tool |
| PATCH | `/admin/tools/{id}/enable` | Enable tool |
| PATCH | `/admin/tools/{id}/disable` | Disable tool |

### Resources

| Method | Path | Description |
|---|---|---|
| GET | `/admin/resources` | List all resources |
| GET | `/admin/resources/{id}` | Get resource by GUID |
| POST | `/admin/resources` | Create resource |
| PUT | `/admin/resources/{id}` | Update resource |
| DELETE | `/admin/resources/{id}` | Delete resource |
| PATCH | `/admin/resources/{id}/enable` | Enable resource |
| PATCH | `/admin/resources/{id}/disable` | Disable resource |

### Prompts

| Method | Path | Description |
|---|---|---|
| GET | `/admin/prompts` | List all prompts |
| GET | `/admin/prompts/{id}` | Get prompt by GUID |
| POST | `/admin/prompts` | Create prompt |
| PUT | `/admin/prompts/{id}` | Update prompt |
| DELETE | `/admin/prompts/{id}` | Delete prompt |
| PATCH | `/admin/prompts/{id}/enable` | Enable prompt |
| PATCH | `/admin/prompts/{id}/disable` | Disable prompt |

## Response Envelope

All management routes return `ApiResponse<T>`:
```json
{
  "success": true,
  "data": { ... }
}
```

Errors:
```json
{
  "success": false,
  "error": "Tool 'xxx' not found."
}
```

## Runtime Layer

| Path | Description |
|---|---|
| `/mcp` | MCP SSE endpoint for AI clients |

## System

| Path | Description |
|---|---|
| `/` | Server info JSON |
| `/openapi/v1.json` | OpenAPI schema (dev mode) |
