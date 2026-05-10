# 08 — Architecture Decision Records

## ADR-001: Manual CQRS Without MediatR
**Decision**: Build CQRS dispatcher manually using reflection + DI.
**Rationale**: Removes external dependency, keeps dispatcher simple and explicitly wired.

---

## ADR-002: Local JSON File Storage for MVP
**Decision**: Store definitions as JSON files, one per entity.
**Rationale**: Zero infrastructure dependencies, human-readable, easy to swap for a DB later.

---

## ADR-003: JsonNode for JSON Schemas
**Decision**: Store MCP schemas (`InputSchema`, `OutputSchema`) as `JsonNode` instead of `string`.
**Rationale**: Prevents nested string escaping issues. Ensures schema validity at the API boundary. Serializes cleanly via System.Text.Json.

---

## ADR-004: Strict Contract Layer Independence
**Decision**: The `Contract` project must not reference the `Domain` project.
**Rationale**: Keeps DTOs pure. Allows publishing the Contract assembly as a standalone NuGet package for clients without dragging in business logic or entities.

---

## ADR-005: Runtime Capability & Execution Seams
**Decision**: Introduce `IToolCapabilityProvider`, `IToolRegistry`, and `IToolExecutor`.
**Rationale**: Prevents `McpToolProvider` from becoming a God Class. Clearly separates reading metadata (capability) from doing work (execution) from storage (repository).
