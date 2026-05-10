# Cấu hình Claude Desktop kết nối với MoMo MCP Server (Bản Runtime Stdio Sạch)

Sau khi tách riêng phần **HTTP API** và phần **Stdio Runtime**, bạn sẽ không còn gặp lỗi rác log khi chạy ứng dụng nữa.

## Bước 1: Mở file cấu hình của Claude Desktop

- **Windows**: `%APPDATA%\Claude\claude_desktop_config.json`
- **macOS**: `~/Library/Application Support/Claude/claude_desktop_config.json`

## Bước 2: Cập nhật cấu hình để gọi file DLL độc lập

Xoá cấu hình dùng `dotnet run` cũ và thay bằng cách gọi trực tiếp file DLL đã được build.

```json
{
  "mcpServers": {
    "momo-crm-mcp": {
      "command": "dotnet",
      "args": [
        "D:\\DevPrograms\\Gits\\mcp-server\\src\\MoMo.McpServer.Runtime.Stdio\\bin\\Debug\\net10.0\\MoMo.McpServer.Runtime.Stdio.dll"
      ]
    }
  },
  "preferences": {
    "coworkScheduledTasksEnabled": true,
    "ccdScheduledTasksEnabled": true,
    "sidebarMode": "task",
    "coworkWebSearchEnabled": true,
    "floatingAtollActive": true
  }
}
```

*(Lưu ý: Bạn phải đảm bảo đã gõ lệnh `dotnet build` project trước khi dùng cấu hình này. File `MoMo.McpServer.Runtime.Stdio.dll` này là file gốc Console Application, hoàn toàn không dính dáng đến Kestrel hay ASP.NET, đảm bảo luồng JSON-RPC của MCP sạch 100%).*

## Bước 3: Khởi động lại Claude Desktop

1. Tắt hoàn toàn Claude Desktop.
2. Bật lại, lúc này Claude sẽ kết nối thẳng vào `Runtime.Stdio`.
3. Mở log (bấm biểu tượng gỡ lỗi hoặc xem `%APPDATA%\Claude\logs`), bạn sẽ thấy kết nối siêu mượt mà không còn bất kỳ lỗi `Unexpected token` nào nữa.
