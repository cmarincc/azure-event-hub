using Azure.Identity;
using EventHubs.Lab.Producer.Features.SendMessageEventHub;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//app.MapPost("/api/eventhub", async ([FromBody] SendMessageEventHubRequest request, IMediator mediator) =>
//{
//    await mediator.Send(new SendMessageEventHubCommand(request));
//    return Results.Ok();
//});

app.MapGet("/", () => "Hello World!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
