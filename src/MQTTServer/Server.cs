using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace MQTTServer
{
    public static class Server
    {
        private static string _username = "admin";
        private static string _password = "Bo!2bjaq";

        public static Task Start_Server_With_WebSockets_Support()
        {
            /*
             * This sample starts a minimal ASP.NET Webserver including a hosted MQTT server.
             */
            var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseKestrel(
                            o =>
                            {
                                // This will allow MQTT connections based on TCP port 1883.
                                o.ListenAnyIP(1883, l => l.UseMqtt());

                                // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
                                // See code below for URI configuration.
                                o.ListenAnyIP(5000); // Default HTTP pipeline
                            });

                        webBuilder.UseStartup<Startup>();
                    });

            return host.RunConsoleAsync();
        }

        sealed class MqttController
        {
            public MqttController()
            {
                // Inject other services via constructor.
            }

            public Task ValidateConnection(ValidatingConnectionEventArgs e)
            {
                e.ReasonCode = MqttConnectReasonCode.Success;
                if (e.ClientId.Length < 10)
                {
                    e.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                }

                if (!e.UserName.Equals(_username))
                {
                    e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }

                if (!e.Password.Equals(_password))
                {
                    e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }

                Console.WriteLine($"Client '{e.ClientId}' wants to connect. Accepting!");
                return Task.CompletedTask;
            }

            public Task OnClientConnected(ClientConnectedEventArgs e)
            {
                Console.WriteLine($"Client '{e.ClientId}' connected.");
                return Task.CompletedTask;
            }

            public Task OnClientDisconnected(ClientDisconnectedEventArgs e)
            {
                Console.WriteLine($"{DateTime.Now} Client Disconnected:ClientId:{e.ClientId}");
                return Task.CompletedTask;
            }

            public Task OnClientSubscribedTopic(ClientSubscribedTopicEventArgs e)
            {
                Console.WriteLine($"{DateTime.Now} Client subscribed topic. ClientId:{e.ClientId} Topic:{e.TopicFilter.Topic} QualityOfServiceLevel:{e.TopicFilter.QualityOfServiceLevel}");
                return Task.CompletedTask;
            }

            public Task OnClientUnsubscribedTopic(ClientUnsubscribedTopicEventArgs e)
            {
                Console.WriteLine($"{DateTime.Now} Client unsubscribed topic. ClientId:{e.ClientId} Topic:{e.TopicFilter.Length}");
                return Task.CompletedTask;
            }

            public Task OnStarted(EventArgs e)
            {
                Console.WriteLine($"{DateTime.Now} Mqtt Server Started on ()...");
                return Task.CompletedTask;
            }

            public Task OnStopped(EventArgs e)
            {
                Console.WriteLine($"{DateTime.Now} Mqtt Server Stopped...");
                return Task.CompletedTask;
            }

            public Task OnInterceptingPublish(InterceptingPublishEventArgs e)
            {
                Console.WriteLine($"{DateTime.Now} Mqtt Server receive message: {Encoding.UTF8.GetString(e.ApplicationMessage?.Payload)} on topic: {e.ApplicationMessage.Topic}");
                return Task.CompletedTask;
            }
        }

        sealed class Startup
        {
            public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, MqttController mqttController)
            {
                app.UseRouting();

                app.UseEndpoints(
                    endpoints =>
                    {
                        endpoints.MapConnectionHandler<MqttConnectionHandler>(
                            "/mqtt",
                            httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                                protocolList => protocolList.FirstOrDefault() ?? string.Empty);
                    });

                app.UseMqttServer(
                    server =>
                    {
                        /*
                         * Attach event handlers etc. if required.
                         */

                        server.ValidatingConnectionAsync += mqttController.ValidateConnection;
                        server.ClientConnectedAsync += mqttController.OnClientConnected;
                        server.ClientDisconnectedAsync += mqttController.OnClientDisconnected;
                        server.ClientSubscribedTopicAsync += mqttController.OnClientSubscribedTopic;
                        server.ClientUnsubscribedTopicAsync += mqttController.OnClientUnsubscribedTopic;
                        server.StartedAsync += mqttController.OnStarted;
                        server.StoppedAsync += mqttController.OnStopped;
                        server.InterceptingPublishAsync += mqttController.OnInterceptingPublish;
                    });
            }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddHostedMqttServer(
                    optionsBuilder =>
                    {
                        optionsBuilder.WithDefaultEndpoint();
                    });

                services.AddMqttConnectionHandler();
                services.AddConnections();

                services.AddSingleton<MqttController>();
            }
        }
    }
}
