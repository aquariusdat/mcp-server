# 07 — Management Layer

## Purpose

The Management Layer is an internal REST API for admins to create and manage MCP definitions. It is NOT exposed to AI clients.

## Design Principles

- **CRUD only** — list, get, create, update, delete
- **State control** — enable/disable individual definitions
- **Thin endpoints** — all logic in Application handlers
- **Clean responses** — `ApiResponse<T>` envelope

## Creating a Tool Example

```http
POST /admin/tools
Content-Type: application/json

{
  "code": "crm_get_transaction",
  "name": "Get Transaction",
  "description": "Fetches a MoMo transaction by transaction ID",
  "type": "lookup",
  "category": "payments",
  "tags": ["payments", "transaction"],
  "enabled": true,
  "inputSchema": "{\"type\":\"object\",\"properties\":{\"transactionId\":{\"type\":\"string\"}},\"required\":[\"transactionId\"]}",
  "outputSchema": "{}",
  "handlerRoute": "payments.get_transaction"
}
```

Response:
```json
{
  "success": true,
  "data": {
    "id": "...",
    "code": "crm_get_transaction",
    ...
  }
}
```

## Enabling/Disabling

```http
PATCH /admin/tools/{id}/enable
PATCH /admin/tools/{id}/disable
```

Disabled tools are excluded from MCP runtime exposure.

## Flow

```
HTTP Request
   → Endpoint (thin — just deserialize + call dispatcher)
      → IDispatcher.SendCommandAsync(command)
         → Handler (business logic)
            → IRepository.SaveAsync(entity)
               → JSON file written to disk
```
