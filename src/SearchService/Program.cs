using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;
using MassTransit;
using SearchService.Consumers;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x => {
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    x.UsingRabbitMq((context, cfg)=>
    {
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async ()=>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception ex)
    {   
        Console.WriteLine(ex);
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
=> HttpPolicyExtensions
.HandleTransientHttpError()
.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
.WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3))
;