# 00 — Project Context

## Purpose

MoMo MCP Server is an internal platform that allows MoMo engineering teams to:

1. **Register** MCP tool, resource, and prompt definitions via a management REST API
2. **Expose** those definitions to AI clients (Claude Desktop, GPT, Gemini) via an MCP-compliant runtime endpoint
3. **Reuse** the platform across teams (CRM first, then other teams)

## Background

The **Model Context Protocol (MCP)** is an open standard for providing structured context, tools, and prompts to Large Language Models. This platform acts as a centralized MCP server for MoMo so that any team can register tools without each team running their own MCP infrastructure.

## Current State (MVP)

- Clean Architecture skeleton with 5 layers
- Manual CQRS (no MediatR)
- Local JSON file storage (one file per definition)
- Full CRUD management API for tools, resources, prompts
- MCP runtime via official C# SDK (`ModelContextProtocol.AspNetCore`)
- 2 sample tools, 1 resource, 1 prompt pre-seeded

## Technology Stack

| Concern | Choice |
|---|---|
| Runtime | .NET 10, ASP.NET Core |
| API style | Minimal API (no controllers) |
| Architecture | Clean Architecture |
| CQRS | Manual (no MediatR) |
| MCP SDK | `ModelContextProtocol.AspNetCore` v1.3.0 |
| Storage | Local JSON files (MVP) |
| DI | `Microsoft.Extensions.DependencyInjection` |
