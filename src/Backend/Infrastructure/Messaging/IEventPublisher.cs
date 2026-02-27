namespace Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event);
}