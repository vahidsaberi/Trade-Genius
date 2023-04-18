using TradeGenius.WebApi.Application.TaskManager.Categories;

namespace TradeGenius.WebApi.Host.Controllers.TaskManager;
public class CategoriesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(TradeGeniusAction.Search, TradeGeniusResource.Categories)]
    [OpenApiOperation("Search categories using available filters.", "")]
    public Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.View, TradeGeniusResource.Categories)]
    [OpenApiOperation("Get category details.", "")]
    public Task<CategoryDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetCategoryRequest(id));
    }

    [HttpPost]
    [MustHavePermission(TradeGeniusAction.Create, TradeGeniusResource.Categories)]
    [OpenApiOperation("Create a new category.", "")]
    public Task<Guid> CreateAsync(CreateCategoryRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Update, TradeGeniusResource.Categories)]
    [OpenApiOperation("Update a category.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateCategoryRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(TradeGeniusAction.Delete, TradeGeniusResource.Categories)]
    [OpenApiOperation("Delete a category.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteCategoryRequest(id));
    }

    [HttpPost("generate-random")]
    [MustHavePermission(TradeGeniusAction.Generate, TradeGeniusResource.Categories)]
    [OpenApiOperation("Generate a number of random categories.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomCategoryRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("delete-random")]
    [MustHavePermission(TradeGeniusAction.Clean, TradeGeniusResource.Categories)]
    [OpenApiOperation("Delete the categories generated with the generate-random call.", "")]
    [ApiConventionMethod(typeof(TradeGeniusApiConventions), nameof(TradeGeniusApiConventions.Search))]
    public Task<string> DeleteRandomAsync()
    {
        return Mediator.Send(new DeleteRandomCategoryRequest());
    }
}
