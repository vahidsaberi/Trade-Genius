using MQTTServer;

await Server.StartServerWithWebSocketsSupport();

Console.WriteLine("Server run...");
Console.WriteLine("Press Enter to exit.");
Console.ReadLine();
