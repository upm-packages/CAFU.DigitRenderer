using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.NumberRenderer
{
    public interface IAsyncNumberRenderer
    {
        UniTask RenderAsync(int value, CancellationToken cancellationToken = default);
    }
}