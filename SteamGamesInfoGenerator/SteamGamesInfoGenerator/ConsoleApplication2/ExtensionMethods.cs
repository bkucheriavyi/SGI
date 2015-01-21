using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public static class ExtensionMethods
    {
        public static Task ForEachAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor)
        {
            var oneAtATime = new SemaphoreSlim(1, 1);
            return Task.WhenAll(
                from item in source
                select ProcessAsync(item, taskSelector, resultProcessor, oneAtATime));
        }

        private static async Task ProcessAsync<TSource, TResult>(TSource item, Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor,
            SemaphoreSlim oneAtATime)
        {
            TResult result = await taskSelector(item);
            await oneAtATime.WaitAsync();
            try
            {
                resultProcessor(item, result);
            }
            finally
            {
                oneAtATime.Release();
            }
        }
    }
}
