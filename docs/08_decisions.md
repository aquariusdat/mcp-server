# 08 — Architecture Decision Records

## ADR-001: Manual CQRS Without MediatR

**Decision**: Build CQRS dispatcher manually using reflection + DI.

**Rationale**:
- Removes external dependency
- Simpler to understand for new .NET developers
- Full control over dispatch behavior
- Slightly more verbose registration but explicit

**Trade-off**: Handler registration in `ApplicationServiceExtensions` must be updated manually when new handlers are added.

---

## ADR-002: Local JSON File Storage for MVP

**Decision**: Store definitions as JSON files, one per entity.

**Rationale**:
- Zero infrastructure dependencies (no DB, no Redis)
- Human-readable files for easy debugging
- Simple to bootstrap for a new team
- Easy to replace with EF Core/PostgreSQL later

**Trade-off**: Not suitable for high concurrency. Acceptable for MVP admin workloads.

---

## ADR-003: Official MCP C# SDK

**Decision**: Use `ModelContextProtocol.AspNetCore` v1.3.0.

**Rationale**:
- Official SDK co-maintained by Microsoft
- Handles full MCP protocol compliance
- Attribute-based tool registration is clean
- HTTP/SSE transport out of the box

**Trade-off**: SDK is evolving; API may change. Pinned to 1.3.0 for stability.

---

## ADR-004: Two Separate Layers

**Decision**: Management Layer (REST) and Runtime Layer (MCP) are distinct concerns in the same host.

**Rationale**:
- Clear separation of admin vs. runtime concerns
- Runtime layer stays thin and reads from the same store
- Simple deployment (one host)

**Future**: May split into separate services if scaling needs differ.

---

## ADR-005: No Auth in MVP

**Decision**: No authentication or authorization.

**Rationale**:
- MVP scope — deploy on internal network
- Auth adds complexity; YAGNI at this stage

**Future**: Add API key or JWT when exposing externally.
