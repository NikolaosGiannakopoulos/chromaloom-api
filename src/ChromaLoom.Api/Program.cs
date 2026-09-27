using ChromaLoom.Api.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UsePipeline();

await app.RunAsync();
