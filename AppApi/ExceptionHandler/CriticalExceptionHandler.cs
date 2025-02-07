using App.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace AppApi.ExceptionHandler
{
    public class CriticalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CriticalException)
            {
                Console.WriteLine("Critical exception occurred");
            }
            return ValueTask.FromResult(false);
        }
    }
}