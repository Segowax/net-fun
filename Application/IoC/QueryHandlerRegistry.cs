using Application.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IoC
{
    public class QueryHandlerRegistry
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryHandlerRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IQueryHandler<TQuery, TResult> Query<TQuery, TResult>()
            where TQuery : IQuery<TResult>
        {
            try
            {
                return _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
