using LibraryManagementSystem.Infastructure;

namespace LibraryManagementSystem.Events
{
    public class EventPublisher : IEventPublisher
    {
        public async Task PublishAsync<TEvent>(TEvent @event)
        {
            if(@event == null) return;

            var consumers = EngineContext.Current.ResolveAll<IConsumer<TEvent>>().ToList();

            foreach (var consumer in consumers)
            {
                await consumer.HandleEventAsync(@event);
            }
        }
    }
}
