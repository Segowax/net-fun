using Application.CommandPattern;
using Application.QueryPattern;

namespace Application;

public interface IMediator
{
    Task<TResult?> SendAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>;

    Task SendAsync<TCommand>(TCommand command)
        where TCommand : class, ICommand;
}
