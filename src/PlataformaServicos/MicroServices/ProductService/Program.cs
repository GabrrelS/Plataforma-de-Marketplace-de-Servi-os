var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

var products = new List<object>();


// QUERY (CQRS)
app.MapGet("/products", () =>
{
    return products;
});


// COMMAND (CQRS)
app.MapPost("/products", (object product) =>
{
    products.Add(product);

    return Results.Ok(new
    {
        message = "Produto criado",
        product
    });
});

app.Run();