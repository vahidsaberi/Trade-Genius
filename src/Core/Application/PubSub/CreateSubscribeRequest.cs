using TradeGenius.WebApi.Application.Common.MQTT;

namespace TradeGenius.WebApi.Application.PubSub;

public class CreateSubscribeRequest : IRequest<bool>
{
    public List<string> Topics { get; set; } = default!;
}

public class CreateSubscribeRequestValidator : CustomValidator<CreateSubscribeRequest>
{
    public CreateSubscribeRequestValidator()
    {
        RuleFor(p => p.Topics)
            .NotEmpty()
            .Must(x => x.All(y => !string.IsNullOrEmpty(y)))
            .WithMessage("The Topic should not be empty or null.");
    }
}

public class CreateSubscribeRequestHandler : IRequestHandler<CreateSubscribeRequest, bool>
{
    private readonly IMqttClientService _service;

    public CreateSubscribeRequestHandler(IMqttClientService service) => _service = service;

    public async Task<bool> Handle(CreateSubscribeRequest request, CancellationToken cancellationToken)
    {
        return await _service.SubscribeAsync(request.Topics);
    }
}