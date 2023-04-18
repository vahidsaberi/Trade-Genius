using DocumentFormat.OpenXml.Office2010.Excel;
using System.Security.Claims;
using TradeGenius.WebApi.Application.TaskManager.Tasks;

namespace TradeGenius.WebApi.Host.Controllers.TaskManager;
public class TasksController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(TradeGeniusAction.Search, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Search tasks using available filters.", "")]
    public Task<PaginationResponse<TaskDto>> SearchAsync(SearchTasksRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.View, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Get task details.", "")]
    public Task<TaskDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetTaskRequest(id));
    }

    [HttpGet("dapper")]
    [MustHavePermission(TradeGeniusAction.View, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Get task details via dapper.", "")]
    public Task<TaskDto> GetDapperAsync(Guid id)
    {
        return Mediator.Send(new GetTaskViaDapperRequest(id));
    }

    [HttpPost]
    [MustHavePermission(TradeGeniusAction.Create, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Create a new task.", "")]
    public Task<Guid> CreateAsync(CreateTaskRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Update, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Update a task.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateTaskRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Delete, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Delete a task.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteTaskRequest(id));
    }

    [HttpGet("report")]
    [MustHavePermission(TradeGeniusAction.View, TradeGeniusResource.Tasks)]
    [OpenApiOperation("Report tasks using user id filters.", "")]
    public async Task<ActionResult<List<TaskDto>>> RreportAsync()
    {
        var userId = User.GetUserId();
        return userId is null
            ? BadRequest()
            : Ok(await Mediator.Send(new GetMyTasksRequest(userId)));
    }
}
