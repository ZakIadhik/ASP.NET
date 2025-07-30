using AppFirst;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



var tasks = new List<TodoTask>();
var nextId = 1;


app.MapGet("/tasks", (bool? completed) =>
{
    var result = completed.HasValue ? tasks.Where(t => t.IsCompleted == completed.Value) : tasks;

    return Results.Ok(result);
}).WithName("GetTasks");



app.MapGet("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    return task is null ? Results.NotFound() : Results.Ok(task);
}).WithName("GetTaskById");


app.MapPost("/tasks", (TodoTask input) =>
{
    if (string.IsNullOrEmpty(input.Title))
        return Results.BadRequest("Title is required.");

    var id = nextId++;
    var task = new TodoTask
    {
        Id = id,
        Title = input.Title,
        IsCompleted = input.IsCompleted
    };

    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
}).WithName("CreateTask");


app.MapPut("/tasks/{id}", (int id, TodoTask input) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    if (string.IsNullOrEmpty(input.Title))
        return Results.BadRequest("Title is required.");

    task.Title = input.Title;
    task.IsCompleted = input.IsCompleted;
    return Results.Ok(task);
}).WithName("UpdateTask");


app.MapDelete("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    tasks.Remove(task);
    return Results.NoContent();
}).WithName("DeleteTask");

app.Run();


