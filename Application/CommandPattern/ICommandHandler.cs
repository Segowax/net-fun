namespace Application.CommandPattern
{
    public interface ICommandHandler<TCommand>
        where TCommand : class
    {
        Task HandleAsync(TCommand command);
    }
}
