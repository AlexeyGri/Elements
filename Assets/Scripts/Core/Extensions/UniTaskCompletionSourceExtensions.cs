using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Extensions
{
    public static class UniTaskCompletionSourceExtensions
    {
        public static UniTaskCompletionSource WithToken(this UniTaskCompletionSource tokenSource, CancellationToken token)
        {
            token.Register(() => tokenSource.TrySetCanceled());
            
            return tokenSource;
        }
        
        public static UniTaskCompletionSource<T> WithToken<T>(this UniTaskCompletionSource<T> tokenSource, CancellationToken token)
        {
            token.Register(() => tokenSource.TrySetCanceled());
            
            return tokenSource;
        }
    }
}