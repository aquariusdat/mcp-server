using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed record CreateResourceCommand(CreateResourceRequest Request) : ICommand<ResourceResponse>;
