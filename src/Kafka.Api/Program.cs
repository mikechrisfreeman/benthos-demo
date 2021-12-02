using Kafka.Api;
#region Setup

var builder = WebApplication.CreateBuilder(args);
var bootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS") ?? "localhost:29092";

builder.Services.AddScoped<IKafkaService>((sp) =>
{
    return new KafkaService(bootstrapServers);
});

var app = builder.Build();

var items = new Dictionary<Guid, Todo>();

#endregion

#region API Endpoints

app.MapGet("/todo/{id}", (Guid id) => {

    if (!items.ContainsKey(id))
        return Results.NotFound();
    return Results.Ok(items[id]);

});

app.MapPost("/todo", async (Todo todo, IKafkaService kafkaService) => {

    items.Add(todo.Id, todo);
    await kafkaService.RaiseEvent("todo_create", todo);
    return Results.Ok(todo);

});

app.MapPut("/todo/{id}", (Guid id, Todo todo) => {

    if (!items.ContainsKey(id))
        return Results.NotFound();
    items[id] = todo;
    return Results.Ok();
    
});

#endregion 

app.Run();