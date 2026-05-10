# 06 — Runtime Layer (MCP Protocol)

## Overview

The Runtime Layer exposes MCP-compliant endpoints consumed by AI clients (Claude Desktop, GPT, Gemini). It uses the official `ModelContextProtocol.AspNetCore` SDK (v1.3.0).

## Separation of Concerns

The runtime layer is strictly separated from the management (storage) layer via two abstractions:

1. **Capability Providers** (`IToolCapabilityProvider`): Expose only the *enabled* definitions for MCP discovery. The runtime cannot modify definitions or see disabled ones.
2. **Executors** (`IToolExecutor` & `IToolRegistry`): Provide the logic to actually *run* a tool when invoked by an MCP client.

## Provider Classes

Located in `Api/Runtime/`, these classes are discovered automatically by the MCP SDK:

| Class | Capability Provider | Purpose |
|---|---|---|
| `McpToolProvider` | `IToolCapabilityProvider` | Lists tools (`momo_list_tools`) and dispatches execution (`momo_invoke_tool`) |
| `McpResourceProvider` | `IResourceCapabilityProvider` | Lists resources (`momo_list_resources`) |
| `McpPromptProvider` | `IPromptCapabilityProvider` | Lists prompts (`momo_list_prompts`) |

## Tool Execution Flow

When an MCP client invokes `momo_invoke_tool`:
1. `McpToolProvider` receives the `toolCode` and JSON `arguments`.
2. It fetches the definition via `IToolCapabilityProvider.FindEnabledToolAsync()`.
3. It resolves the executor via `IToolRegistry.Resolve(tool.HandlerRoute)`.
4. It calls `IToolExecutor.ExecuteAsync(context)`.
5. The resulting `ToolExecutionResult` is serialized and returned to the MCP client.

*(MVP Note: The default `IToolRegistry` is a `NoOpToolRegistry` that logs and returns null).*
