var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "Marketplace:";
});

builder.Services.AddScoped<ProductCacheService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var products = new List<object>();

app.MapGet("/products", async (ProductCacheService cache) =>
{
    var cacheKey = "products:list";

    var cachedProducts = await cache.GetAsync(cacheKey);

    if (cachedProducts != null)
    {
        Console.WriteLine("CACHE HIT");
        return Results.Ok(cachedProducts);
    }

    Console.WriteLine("CACHE MISS");

    await cache.SetAsync(cacheKey, products);

    return Results.Ok(products);
});

app.MapPost("/products", async (object product, ProductCacheService cache) =>
{
    products.Add(product);

    await cache.RemoveAsync("products:list");

    return Results.Ok(new
    {
        message = "Produto criado",
        product
    });
});

app.Run();