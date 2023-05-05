using TradeGenius.WebApi.Application.Common.MQTT;

namespace TradeGenius.WebApi.Application.PubSub;

public class CreateUnsubscribeRequest : IRequest<bool>
{
    public List<string> Topics { get; set; } = default!;
}

public class CreateUnsubscribeRequestValidator : CustomValidator<CreateUnsubscribeRequest>
{
    public CreateUnsubscribeRequestValidator()
    {
        RuleFor(p => p.Topics)
            .NotEmpty()
            .Must(x => x.All(y => !string.IsNullOrEmpty(y)))
            .WithMessage("The Topic should not be empty or null.");
    }
}

public class CreateUnsubscribeRequestHandler : IRequestHandler<CreateUnsubscribeRequest, bool>
{
    private readonly IMqttClientService _service;

    public CreateUnsubscribeRequestHandler(IMqttClientService service) => _service = service;

    public async Task<bool> Handle(CreateUnsubscribeRequest request, CancellationToken cancellationToken)
    {
        return await _service.UnsubscribeAsync(request.Topics);
    }
}
