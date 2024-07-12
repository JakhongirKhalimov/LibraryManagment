namespace LibraryManagementSystem.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event);
    }
}
