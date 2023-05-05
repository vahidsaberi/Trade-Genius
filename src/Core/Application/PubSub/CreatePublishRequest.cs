using TradeGenius.WebApi.Application.Common.MQTT;

namespace TradeGenius.WebApi.Application.PubSub;

public class CreatePublishRequest : IRequest<bool>
{
    public string Topic { get; set; } = default!;
    public string Content { get; set; } = default!;
}

public class CreatePublishRequestValidator : CustomValidator<CreatePublishRequest>
{
    public CreatePublishRequestValidator()
    {
        RuleFor(p => p.Topic)
            .NotEmpty();

        RuleFor(p => p.Content)
                .NotEmpty();
    }
}

public class CreatePublishRequestHandler : IRequestHandler<CreatePublishRequest, bool>
{
    private readonly IMqttClientService _service;

    public CreatePublishRequestHandler(IMqttClientService service) => _service = service;

    public async Task<bool> Handle(CreatePublishRequest request, CancellationToken cancellationToken)
    {
        var topic = Enum.Parse<MqttTopics>(request.Topic);
        return await _service.PublishAsync(topic, request.Content);
    }
}
