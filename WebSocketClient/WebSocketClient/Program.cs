using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using var client = new ClientWebSocket();
        await client.ConnectAsync( new Uri("ws://localhost:5096/ws"), CancellationToken.None );
        Console.WriteLine("Connected to server");

        // Send a message
        var message = "ping";
        var bytes = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine($"Sent: {message}");

        // Receive a message
        var buffer = new byte[1024];
        var result = await client.ReceiveAsync(buffer, CancellationToken.None);
        Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer, 0, result.Count));

        // Send "/work/start" message with delay
        await Task.Delay(TimeSpan.FromSeconds(2));

        message = "/work/start";
        bytes = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);

        // Receive a message
        result = await client.ReceiveAsync(buffer, CancellationToken.None);
        Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer, 0, result.Count));

        // Receive a message
        result = await client.ReceiveAsync(buffer, CancellationToken.None);
        Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer, 0, result.Count));

        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", CancellationToken.None);
    }
}
