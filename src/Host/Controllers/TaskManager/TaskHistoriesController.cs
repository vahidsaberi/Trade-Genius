using TradeGenius.WebApi.Application.TaskManager.TaskHistories;
using TradeGenius.WebApi.Application.TaskManager.TaskHistoryHistories;

namespace TradeGenius.WebApi.Host.Controllers.TaskManager;
public class TaskHistoriesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(TradeGeniusAction.Search, TradeGeniusResource.TaskHistories)]
    [OpenApiOperation("Search tasks using available filters.", "")]
    public Task<PaginationResponse<TaskHistoryDto>> SearchAsync(SearchTaskHistoriesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.View, TradeGeniusResource.TaskHistories)]
    [OpenApiOperation("Get task details.", "")]
    public Task<TaskHistoryDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetTaskHistoryRequest(id));
    }

    [HttpPost]
    [MustHavePermission(TradeGeniusAction.Create, TradeGeniusResource.TaskHistories)]
    [OpenApiOperation("Create a new task.", "")]
    public Task<Guid> CreateAsync(CreateTaskHistoryRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Update, TradeGeniusResource.TaskHistories)]
    [OpenApiOperation("Update a task.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateTaskHistoryRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Delete, TradeGeniusResource.TaskHistories)]
    [OpenApiOperation("Delete a task.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteTaskHistoryRequest(id));
    }
}
