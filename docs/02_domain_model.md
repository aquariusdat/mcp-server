# 02 — Domain Model

## Base Entity

`BaseDefinition` — shared fields for all three MCP definition types.

| Field | Type | Purpose |
|---|---|---|
| `Id` | Guid | Unique identifier |
| `Code` | string | Machine-readable key (unique, snake_case) |
| `Name` | string | Human display name |
| `Description` | string | Purpose description |
| `Type` | string | Arbitrary type tag (e.g., "lookup", "search") |
| `Category` | string | Logical grouping (e.g., "crm", "finance") |
| `Tags` | List\<string\> | Free-form tags |
| `Enabled` | bool | Whether exposed to MCP clients |
| `Version` | int | Increments on each update |
| `CreatedAt` | DateTimeOffset | Creation timestamp |
| `UpdatedAt` | DateTimeOffset | Last update timestamp |

## McpToolDefinition

Extends `BaseDefinition`.

| Field | Type | Purpose |
|---|---|---|
| `InputSchema` | string (JSON) | JSON Schema for tool input parameters |
| `OutputSchema` | string (JSON) | JSON Schema for tool output |
| `HandlerRoute` | string | Internal dispatch key |

## McpResourceDefinition

Extends `BaseDefinition`.

| Field | Type | Purpose |
|---|---|---|
| `Uri` | string | Stable resource URI (e.g., `momo://crm/customers`) |
| `MimeType` | string | Content MIME type |
| `HandlerRoute` | string | Internal dispatch key |

## McpPromptDefinition

Extends `BaseDefinition`.

| Field | Type | Purpose |
|---|---|---|
| `Template` | string | Prompt template with `{{arg}}` placeholders |
| `Arguments` | List\<PromptArgumentDefinition\> | Declared template arguments |

## Domain Rules

- `Enable()` / `Disable()` — state methods that set `Enabled` and update `UpdatedAt`
- `BumpVersion()` — increments `Version` and updates `UpdatedAt`
- All entities are identified by `Guid` (not database auto-increment)
