using Users.WebApi.Configurations;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureServices();

var app = builder
    .Build()
    .UsePipeline();

app.Run();