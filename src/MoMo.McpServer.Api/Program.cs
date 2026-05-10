using MoMo.McpServer.Api.Endpoints.Management;
using MoMo.McpServer.Api.Runtime;
using MoMo.McpServer.Application.DependencyInjection;
using MoMo.McpServer.Infrastructure.DependencyInjection;
using MoMo.McpServer.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// ── Configuration ──────────────────────────────────────────────────────────────
// Resolve storage base path — defaults to {ContentRootPath}/data
// Note: treat empty string the same as null (not configured)
var configuredPath = builder.Configuration["Storage:BasePath"];
var storagePath = string.IsNullOrWhiteSpace(configuredPath)
    ? Path.Combine(builder.Environment.ContentRootPath, "data")
    : configuredPath;

builder.Services.Configure<StorageOptions>(opts => opts.BasePath = storagePath);

// ── Application + Infrastructure services ────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// ── MCP Runtime Server (HTTP/SSE transport via ModelContextProtocol.AspNetCore) ──
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly(typeof(McpToolProvider).Assembly);

// ── OpenAPI (Scalar / Swagger-compatible) ─────────────────────────────────────
builder.Services.AddOpenApi();

// ── Logging ───────────────────────────────────────────────────────────────────
builder.Services.AddLogging();

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ── Management Layer routes ───────────────────────────────────────────────────
app.MapToolEndpoints();
app.MapResourceEndpoints();
app.MapPromptEndpoints();

// ── Runtime Layer: MCP SSE endpoint ──────────────────────────────────────────
// Claude Desktop / GPT / Gemini connects here.
// The SDK handles MCP handshake, tools/list, tools/call, etc.
app.MapMcp("/mcp");

// ── Health / info ─────────────────────────────────────────────────────────────
app.MapGet("/", () => Results.Ok(new
{
    name = "MoMo MCP Server",
    version = "1.0.0",
    managementApi = "/admin/{tools|resources|prompts}",
    mcpRuntime = "/mcp",
    openApi = "/openapi/v1.json",
}))
.WithName("ServerInfo")
.WithTags("System");

app.Run();
