﻿using Application.CommandPattern;
using Application.Query;
using Application.QueryPattern;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult?> SendAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
    {
        try
        {
            var service = _serviceProvider
                .GetRequiredService<IQueryHandler<TQuery, TResult>>();

            return await service.HandleAsync(query);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public async Task SendAsync<TCommand>(TCommand command)
        where TCommand : class, ICommand
    {
        try
        {
            var service = _serviceProvider
                .GetRequiredService<ICommandHandler<TCommand>>();

            await service.HandleAsync(command);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}
