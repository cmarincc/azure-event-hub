using MediatR;

namespace EventHubs.Lab.Producer.Features.SendMessageEventHub;

public record SendMessageEventHubCommand(SendMessageEventHubRequest MessageRequest): IRequest;