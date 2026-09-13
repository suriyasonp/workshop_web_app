using TodoApi.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var todos = new List<TodoGetDto>
{
    new(1, "Learn C#", true),
    new(2, "Learn ASP.NET Core", false),
    new(3, "Build a web API", false)
};

app.MapGet("/api/todos", () => Results.Ok(todos));

app.MapGet("/api/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);

    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/api/todos", (TodoPostDto dto) =>
{
    var nextId = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;

    var todo = new TodoGetDto(nextId, dto.Title, false);
    todos.Add(todo);

    return Results.Created($"/api/todos/{todo.Id}", todo);
});

app.MapPut("/api/todos/{id}", (int id, TodoPutDto dto) =>
{
    try
    {
        var index = todos.FindIndex(t => t.Id == id);
        if (index == -1) return Results.NotFound();

        todos[index] = todos[index] with
        {
            Title = dto.Title,
            IsCompleted = dto.IsCompleted
        };
    
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }

});
app.Run();