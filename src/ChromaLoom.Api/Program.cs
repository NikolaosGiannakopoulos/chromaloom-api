using ChromaLoom.Api.Setup.Pipeline.Orchestration;
using ChromaLoom.Api.Setup.Services.Registration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UsePipeline();

await app.RunAsync();
