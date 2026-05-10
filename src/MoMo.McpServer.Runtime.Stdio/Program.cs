using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoMo.McpServer.Application.DependencyInjection;
using MoMo.McpServer.Infrastructure.DependencyInjection;
using MoMo.McpServer.Runtime.Stdio.Providers;

// We use Generic Host (not WebHost) to ensure a pure Console Application environment
var builder = Host.CreateApplicationBuilder(args);

// ── PREVENT STDOUT POLLUTION (CRITICAL) ───────────────────────────────────────
// Claude Desktop communicates via standard input/output.
// Any non-JSON text printed to stdout (e.g. ASP.NET "Hosting started") will crash Claude.
builder.Logging.ClearProviders();
// If you need logging, add a file logger or a custom ConsoleLogger that writes exclusively to Console.Error.

// ── Application Layer ─────────────────────────────────────────────────────────
builder.Services.AddApplicationServices();

// ── Infrastructure Layer ──────────────────────────────────────────────────────
// Using AppContext.BaseDirectory so it can resolve the local JSON storage folder
// even when run as an absolute path executable by Claude Desktop.
builder.Services.AddInfrastructureServices(builder.Configuration, AppContext.BaseDirectory);

// ── MCP Server Configuration ──────────────────────────────────────────────────
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(McpToolProvider).Assembly);

var host = builder.Build();

// Run the MCP Server indefinitely, listening on stdio.
await host.RunAsync();
