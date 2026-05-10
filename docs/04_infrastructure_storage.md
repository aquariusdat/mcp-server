# 04 — Infrastructure / Storage

## Design

- One JSON file per entity: `{BasePath}/{type}/{guid}.json`
- Three directories: `tools/`, `resources/`, `prompts/`
- Files created on first write; directories auto-created
- Corrupt/missing files are silently skipped (graceful degradation)

## Configuration

Set in `appsettings.json`:
```json
{
  "Storage": {
    "BasePath": ""
  }
}
```

Empty `BasePath` defaults to `{ContentRootPath}/data` at runtime (resolved in `Program.cs`).

## File Format

Each entity is a plain JSON file, human-readable. Example:
```json
{
  "Id": "11111111-...",
  "Code": "crm_get_customer",
  "Name": "Get Customer",
  "Enabled": true,
  "InputSchema": "{...}",
  ...
}
```

## Implementations

| Interface | Implementation |
|---|---|
| `IToolRepository` | `JsonFileToolRepository` |
| `IResourceRepository` | `JsonFileResourceRepository` |
| `IPromptRepository` | `JsonFilePromptRepository` |

All implement the same interface contract with `GetAllAsync`, `GetByIdAsync`, `GetByCodeAsync`, `SaveAsync`, `DeleteAsync`.

## Replacing Storage

To replace file storage with a database:
1. Create a new implementation of `IToolRepository` etc. in Infrastructure
2. Update `InfrastructureServiceExtensions` to register the new implementation
3. Zero changes required in Domain, Contract, or Application

## DI Registration

Call `services.AddInfrastructureServices()` from `Program.cs`.
