using System.Threading;

namespace Southbyte
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelAndDisposeIfNotNull(this CancellationTokenSource cancellationTokenSource)
        {
            if(cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested)
                return;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        public static CancellationToken RefreshToken(this CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.CancelAndDisposeIfNotNull();
            cancellationTokenSource = new CancellationTokenSource();
            return cancellationTokenSource.Token;
        }
    }
}