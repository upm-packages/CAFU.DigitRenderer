using CAFU.Core;
using CAFU.NumberRenderer.Entity;

namespace CAFU.NumberRenderer.UseCase
{
    internal class NumberRendererUseCase : IInitializeNotifiable
    {
        public NumberRendererUseCase(INumberRenderer numberRenderer, InitialState initialState)
        {
            NumberRenderer = numberRenderer;
            InitialState = initialState;
        }

        private INumberRenderer NumberRenderer { get; }
        private InitialState InitialState { get; }

        void IInitializeNotifiable.OnInitialize()
        {
            if (InitialState.ShouldRender)
            {
                NumberRenderer.Render(InitialState.Value);
            }
        }
    }
}