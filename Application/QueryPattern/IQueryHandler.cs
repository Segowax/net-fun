﻿using Application.QueryPattern;

namespace Application.Query;

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult?> HandleAsync(TQuery query);
}
