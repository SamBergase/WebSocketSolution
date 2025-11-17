## Structure

WebSocketSolution/
├── WebSocketServer/      (ASP.NET Core Empty)
│   ├── Program.cs
│   ├── WebSocketServer.csproj
│   └── wwwroot/
│       └── index.html
└── WebSocketClient/      (C# Console App)
    ├── Program.cs
    └── WebSocketClient.csproj

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2026 or another IDE supporting .NET 8

## How to Run

### Server (WebSocketServer)

1. Open a terminal window and navigate to `WebSocketServer` directory.
2. run the command:
         dotnet run
3. Wait for the server to start listening on `http://localhost:5096`.

### Browser Client

1. Open a NEW terminal window.
2. Go to the WebSocketServer directory.
3. run the command:
         dotnet run

### Start the browser client

1. Open a webbrowser.
2. Go to:
         http://localhost:5096/index.html

## Notes

- Server and clients can run simultaneously.
- Browser client uses JavaScript WebSocket API.
- C# client uses `System.Net.WebSockets.ClientWebSocket`.
- This program is rather dumb, as it works only with in-line code and echoes messages back.
  An improvment would be to use event driven architecture and allow for multiple requests and responses.dot

## License

MIT License
