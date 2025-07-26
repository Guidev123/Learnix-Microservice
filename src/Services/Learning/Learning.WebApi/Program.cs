using Learning.WebApi.Configurations;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureServices();

var app = builder
    .Build()
    .UsePipeline(builder);

app.Run();