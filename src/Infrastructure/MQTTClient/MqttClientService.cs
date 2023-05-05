using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System.Text;
using TradeGenius.WebApi.Application.Common.MQTT;
using TradeGenius.WebApi.Application.Crypto.Coins;
using TradeGenius.WebApi.Infrastructure.MQTTClient.SettingsModel;

namespace TradeGenius.WebApi.Infrastructure.MQTTClient;

public class MqttClientService : IMqttClientService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private readonly ILogger<MqttClientService> _logger;
    private readonly MqttSettings _settings;

    public MqttClientService(MqttClientOptions options, ILogger<MqttClientService> logger, IOptions<MqttSettings> settings)
    {
        this._options = options;
        _mqttClient = new MqttFactory().CreateMqttClient();
        _logger = logger;
        _settings = settings.Value;

        ConfigureMqttClient();
    }

    private void ConfigureMqttClient()
    {
        _mqttClient.ConnectedAsync += HandleConnectedAsync;
        _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        _mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
    }

    public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
    {
        _logger.LogInformation("connected");

        await this.SubscribeAsync(_settings.Topics);
    }

    public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
    {

        _logger.LogInformation("HandleDisconnected");
        #region Reconnect_Using_Event :https://github.com/dotnet/MQTTnet/blob/master/Samples/Client/Client_Connection_Samples.cs
        /*
        * This sample shows how to reconnect when the connection was dropped.
        * This approach uses one of the events from the client.
        * This approach has a risk of dead locks! Consider using the timer approach (see sample).
        * The following reconnection code "Reconnect_Using_Timer" is recommended
       */
        //if (eventArgs.ClientWasConnected)
        //{
        //    // Use the current options as the new options.
        //    await mqttClient.ConnectAsync(mqttClient.Options);
        //}
        #endregion
        await Task.CompletedTask;
    }

    public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        var message = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);

        switch(Enum.Parse<MqttTopics>(eventArgs.ApplicationMessage.Topic))
        {
            case MqttTopics.CoinCap:
                var data = JsonConvert.DeserializeObject<List<CoinCapDto>>(message);
                break;

        }

        throw new System.NotImplementedException();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.ConnectAsync(_options);

        #region Reconnect_Using_Timer:https://github.com/dotnet/MQTTnet/blob/master/Samples/Client/Client_Connection_Samples.cs
        /* 
         * This sample shows how to reconnect when the connection was dropped.
         * This approach uses a custom Task/Thread which will monitor the connection status.
        * This is the recommended way but requires more custom code!
       */
        _ = Task.Run(
       async () =>
       {
           // // User proper cancellation and no while(true).
           while (true)
           {
               try
               {
                   // This code will also do the very first connect! So no call to _ConnectAsync_ is required in the first place.
                   if (!await _mqttClient.TryPingAsync())
                   {
                       await _mqttClient.ConnectAsync(_mqttClient.Options, CancellationToken.None);

                       // Subscribe to topics when session is clean etc.
                       _logger.LogInformation("The MQTT client is connected.");
                   }
               }
               catch (Exception ex)
               {
                   // Handle the exception properly (logging etc.).
                   _logger.LogError(ex, "The MQTT client  connection failed");
               }
               finally
               {
                   // Check the connection state every 5 seconds and perform a reconnect if required.
                   await Task.Delay(TimeSpan.FromSeconds(5));
               }
           }
       });
        #endregion

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            var disconnectOption = new MqttClientDisconnectOptions
            {
                Reason = MqttClientDisconnectReason.NormalDisconnection,
                ReasonString = "NormalDiconnection"
            };
            await _mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
        }
        await _mqttClient.DisconnectAsync();
    }

    public async Task<bool> SubscribeAsync(List<string> topics)
    {
        try
        {
            if (topics is null || !topics.Any()) return false;

            var so = new MqttClientSubscribeOptions();

            var tpcs = new List<MqttTopicFilter>();

            topics.ForEach(topic =>
            {
                tpcs.Add(new MqttTopicFilter
                {
                    Topic = topic,
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                });
            });

            so.TopicFilters = tpcs;

            var result = await _mqttClient.SubscribeAsync(so);

            return result.Items.Any();
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UnsubscribeAsync(List<string> topics)
    {

        try
        {
            if (topics is null || !topics.Any()) return false;

            var so = new MqttClientUnsubscribeOptions
            {
                TopicFilters = topics
            };

            var result = await _mqttClient.UnsubscribeAsync(so);

            return result.Items.Any();
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> PublishAsync(MqttTopics topic, string message)
    {
        try
        {
            var msg = new MqttApplicationMessage
            {
                Topic = topic.ToString(),
                Payload = Encoding.UTF8.GetBytes(message),
                QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce,
                Retain = false
            };

            var result = await _mqttClient.PublishAsync(msg);

            return result.IsSuccess;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
