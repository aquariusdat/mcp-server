using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Application.Resources.Queries;

public sealed record ListResourcesQuery(bool? EnabledOnly = null) : IQuery<IReadOnlyList<ResourceResponse>>;
