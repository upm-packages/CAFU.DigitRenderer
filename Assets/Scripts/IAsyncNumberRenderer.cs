using System.Threading;
using UniRx.Async;

namespace CAFU.NumberRenderer
{
    public interface IAsyncNumberRenderer
    {
        UniTask RenderAsync(int value, CancellationToken cancellationToken = default);
    }
}