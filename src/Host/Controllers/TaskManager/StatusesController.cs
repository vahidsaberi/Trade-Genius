using TradeGenius.WebApi.Application.TaskManager.Statuses;

namespace TradeGenius.WebApi.Host.Controllers.TaskManager;
public class StatusesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(TradeGeniusAction.Search, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Search statuses using available filters.", "")]
    public Task<PaginationResponse<StatusDto>> SearchAsync(SearchStatusesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.View, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Get status details.", "")]
    public Task<StatusDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetStatusRequest(id));
    }

    [HttpPost]
    [MustHavePermission(TradeGeniusAction.Create, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Create a new status.", "")]
    public Task<Guid> CreateAsync(CreateStatusRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Update, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Update a status.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateStatusRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Delete, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Delete a status.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteStatusRequest(id));
    }

    [HttpPost("generate-random")]
    [MustHavePermission(TradeGeniusAction.Generate, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Generate a number of random statuses.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomStatusRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("delete-random")]
    [MustHavePermission(TradeGeniusAction.Clean, TradeGeniusResource.Statuses)]
    [OpenApiOperation("Delete the statuses generated with the generate-random call.", "")]
    [ApiConventionMethod(typeof(TradeGeniusApiConventions), nameof(TradeGeniusApiConventions.Search))]
    public Task<string> DeleteRandomAsync()
    {
        return Mediator.Send(new DeleteRandomStatusRequest());
    }
}
