using TradeGenius.WebApi.Application.Common.Interfaces.MQTT;

namespace TradeGenius.WebApi.Application.PubSub;

public class CreatePubSubRequest : IRequest<bool>
{
    public string Topic { get; set; } = default!;
    public string Content { get; set; } = default!;
}

public class CreatePubSubRequestValidator : CustomValidator<CreatePubSubRequest>
{
    public CreatePubSubRequestValidator()
    {
        RuleFor(p => p.Topic)
            .NotEmpty();

        RuleFor(p => p.Content)
                .NotEmpty();
    }
}

public class CreatePubSubRequestHandler : IRequestHandler<CreatePubSubRequest, bool>
{
    private readonly IMqttClientService _service;

    public CreatePubSubRequestHandler(IMqttClientService service) => _service = service;

    public async Task<bool> Handle(CreatePubSubRequest request, CancellationToken cancellationToken)
    {
        _service.MessageSendAsync(request.Topic, request.Content);

        return true;
    }
}
