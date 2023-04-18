namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class TaskByTitleSpec : Specification<Domain.TaskManager.Task>, ISingleResultSpecification
{
    public TaskByTitleSpec(string title) =>
        Query.Where(b => b.Title == title);
}