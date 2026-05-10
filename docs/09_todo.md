# 09 — Todo / Roadmap

## MVP Complete ✅

- [x] Solution structure (5 projects)
- [x] Domain entities (Tool, Resource, Prompt)
- [x] Contract DTOs
- [x] Manual CQRS (commands, queries, handlers, dispatcher)
- [x] Repository interfaces
- [x] JSON file storage
- [x] Management CRUD API (tools, resources, prompts)
- [x] MCP Runtime Layer (via official C# SDK)
- [x] Seed data (2 tools, 1 resource, 1 prompt)
- [x] OpenAPI support
- [x] Documentation (docs/, README, AGENT.md)

## Next Steps

### Phase 2: Real Tool Dispatch
- [ ] Implement `HandlerRoute` dispatch system (HTTP call-out to real tool handlers)
- [ ] Define tool invocation contract
- [ ] Add tool response schema validation

### Phase 3: Production Readiness
- [ ] Replace file storage with PostgreSQL (EF Core)
- [ ] Add authentication (API key for management, JWT for runtime)
- [ ] Add rate limiting
- [ ] Add health checks endpoint (`/health`)
- [ ] Add structured logging (Serilog)
- [ ] Add OpenTelemetry metrics/tracing

### Phase 4: Multi-Team Support
- [ ] Add team/namespace scoping to definitions
- [ ] Add per-team API keys
- [ ] Add admin UI (Blazor or Next.js)
- [ ] Add definition versioning / rollback

### Phase 5: Advanced MCP Features
- [ ] Expose real MCP Resources (not just meta-tools)
- [ ] Expose real MCP Prompts via `prompts/get`
- [ ] Support MCP sampling
- [ ] Support MCP roots
