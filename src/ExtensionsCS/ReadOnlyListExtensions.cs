using System;
using System.Collections.Generic;

namespace TradingEngineServer.Extensions
{
    public static class ReadOnlyListExtensions
    {
        public static void ForEach<T>(this IReadOnlyList<T> list, Action<T> lambda)
        {
            foreach (var item in list)
                lambda(item);
        }
    }
}
