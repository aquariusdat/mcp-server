namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Executes a single MCP tool given its invocation context.
/// Implementations are registered per tool code in the IToolRegistry.
///
/// MVP: No concrete executors exist yet — the NoOpToolRegistry returns null for all codes.
/// Future: Add concrete executor classes in Infrastructure/Executors/ or a dedicated project.
///
/// Example:
///   "crm.get_customer" → CrmGetCustomerExecutor
///   "payments.get_transaction" → GetTransactionExecutor
/// </summary>
public interface IToolExecutor
{
    /// <summary>
    /// Executes the tool with the provided context.
    /// Should not throw for expected failures — return ToolExecutionResult.Error() instead.
    /// </summary>
    Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default);
}
