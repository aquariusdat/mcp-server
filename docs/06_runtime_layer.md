# 06 — Runtime Layer (MCP Protocol)

## Overview

The Runtime Layer exposes MCP-compliant endpoints consumed by AI clients (Claude Desktop, GPT, Gemini).

It uses the official `ModelContextProtocol.AspNetCore` SDK (v1.3.0).

## How It Works

1. `AddMcpServer().WithHttpTransport().WithToolsFromAssembly(...)` is called in `Program.cs`
2. The SDK discovers classes decorated with `[McpServerToolType]`
3. Methods decorated with `[McpServerTool]` are exposed as MCP tools
4. The SDK handles the full MCP protocol: `initialize`, `tools/list`, `tools/call`, SSE transport

## Provider Classes

Located in `Api/Runtime/`:

| Class | MCP Tool Name | Description |
|---|---|---|
| `McpToolProvider` | `momo_list_tools` | Lists enabled tool definitions |
| `McpResourceProvider` | `momo_list_resources` | Lists enabled resource definitions |
| `McpPromptProvider` | `momo_list_prompts` | Lists enabled prompt definitions |

## Connecting Claude Desktop

Add to `claude_desktop_config.json`:
```json
{
  "mcpServers": {
    "momo": {
      "url": "http://localhost:5000/mcp"
    }
  }
}
```

## Architecture Notes

- Runtime providers are **thin** — they only read from repositories
- No business logic in providers
- New tools don't require code changes — register via `/admin/tools` API
- Future: implement real tool dispatch (HTTP, gRPC, serverless) via `HandlerRoute`

## Extending the Runtime

To add a real callable tool:
1. Register the tool via `POST /admin/tools` with an appropriate `HandlerRoute`
2. Add a new `[McpServerTool]`-decorated method in a provider class
3. Method reads the stored definition, dispatches to the handler
4. Return the result as a string (MCP content format)
