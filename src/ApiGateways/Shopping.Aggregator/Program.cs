using Common.Logging;
using Serilog;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri("http://localhost:8000"))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();
    //c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]));

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri("http://localhost:8001"))
    .AddHttpMessageHandler<LoggingDelegatingHandler>(); ;
    //c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]));

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri("http://localhost:8004"))
    .AddHttpMessageHandler<LoggingDelegatingHandler>(); ;
    //c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.Run();
