using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static string GenerateRandomString(int length)
{
    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@$€";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}

// Serve static files (index.html)
app.UseStaticFiles();

// Enable WebSockets
app.UseWebSockets();

// WebSocket endpoint
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var buffer = new byte[1024];
        bool workDelayStarted = false;
        string hash = "";

        while (socket.State == System.Net.WebSockets.WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
            {
                await socket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
            }
            else
            {
                var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received from client: {message}");
                if (message.ToLower().Contains("ping"))
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    await socket.SendAsync(System.Text.Encoding.UTF8.GetBytes("pong"),
                        result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
                if (message.ToLower().Contains("/work/start"))
                {
                    hash = GenerateRandomString(16);
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    await socket.SendAsync(System.Text.Encoding.UTF8.GetBytes("WorkStarted: " + hash),
                        result.MessageType, result.EndOfMessage, CancellationToken.None);
                    workDelayStarted = true;
                }
                if (workDelayStarted)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await socket.SendAsync(System.Text.Encoding.UTF8.GetBytes("WorkFinished: " + hash),
                        result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
            }
        }
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();
