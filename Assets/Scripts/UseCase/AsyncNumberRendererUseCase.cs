using System.Threading;
using CAFU.Core;
using CAFU.NumberRenderer.Entity;
using Cysharp.Threading.Tasks;

namespace CAFU.NumberRenderer.UseCase
{
    internal class AsyncNumberRendererUseCase : IAsyncInitializeNotifiable
    {
        public AsyncNumberRendererUseCase(IAsyncNumberRenderer asyncNumberRenderer, InitialState initialState)
        {
            AsyncNumberRenderer = asyncNumberRenderer;
            InitialState = initialState;
        }

        private IAsyncNumberRenderer AsyncNumberRenderer { get; }
        private InitialState InitialState { get; }

        async UniTask IAsyncInitializeNotifiable.OnInitializeAsync(CancellationToken cancellationToken)
        {
            if (InitialState.ShouldRender)
            {
                await AsyncNumberRenderer.RenderAsync(InitialState.Value, cancellationToken);
            }
        }
    }
}