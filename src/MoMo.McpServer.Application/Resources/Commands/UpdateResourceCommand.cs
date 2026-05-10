using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed record UpdateResourceCommand(Guid Id, UpdateResourceRequest Request) : ICommand<ResourceResponse?>;
