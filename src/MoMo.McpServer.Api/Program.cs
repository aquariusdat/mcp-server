using MoMo.McpServer.Api.Endpoints.Management;
using MoMo.McpServer.Application.DependencyInjection;
using MoMo.McpServer.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ── Application layer: CQRS handlers + runtime capability providers ───────────
builder.Services.AddApplicationServices();

// ── Infrastructure layer: storage, repositories, runtime stubs ────────────────
// StorageOptions configuration is encapsulated inside AddInfrastructureServices.
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.ContentRootPath);

// ── OpenAPI ────────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ── Management Layer: Admin CRUD routes ───────────────────────────────────────
app.MapToolEndpoints();
app.MapResourceEndpoints();
app.MapPromptEndpoints();

// ── Runtime Layer: MCP protocol endpoint ──────────────────────────────────────
// Removed: The MCP Runtime is now hosted in MoMo.McpServer.Runtime.Stdio project.

// ── Server info ───────────────────────────────────────────────────────────────
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
