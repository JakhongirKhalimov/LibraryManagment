namespace LibraryManagementSystem.Events
{
    public interface IConsumer<TEvent>
    {
        Task HandleEventAsync(TEvent @event);
    }
}
