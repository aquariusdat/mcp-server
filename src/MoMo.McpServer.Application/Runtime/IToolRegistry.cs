namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Registry that maps tool codes to their concrete executors.
/// Provides the dispatch seam between MCP tool invocation and real handler logic.
///
/// Resolve(code) returns null when no executor is registered for that code.
/// The caller is responsible for handling the null case (e.g. return a "not implemented" error).
///
/// MVP implementation: NoOpToolRegistry — always returns null.
/// Future: Register concrete IToolExecutor implementations keyed by HandlerRoute/Code.
/// </summary>
public interface IToolRegistry
{
    /// <summary>
    /// Resolves the executor for the given tool code.
    /// Returns null if no executor is registered for this code.
    /// </summary>
    IToolExecutor? Resolve(string toolCode);
}
