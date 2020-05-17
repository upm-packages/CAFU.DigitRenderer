using System.Collections.Generic;
using UnityEngine;

namespace CAFU.NumberRenderer.Presenter
{
    internal class NumberRenderingPresenter<TComponent, TImage> :
        NumberRenderingPresenterBase<TComponent, TImage>,
        INumberRenderer
        where TComponent : Component
        where TImage : Object
    {
        public NumberRenderingPresenter(IRendererComponentsProvider<TComponent> rendererComponentsProvider, IRenderableImagesProvider<TImage> renderableImagesProvider, IEmptyDigitsControllable emptyDigitsControllable) : base(rendererComponentsProvider, renderableImagesProvider, emptyDigitsControllable)
        {
            RenderableImagesProvider = renderableImagesProvider;
        }

        private IRenderableImagesProvider<TImage> RenderableImagesProvider { get; }

        protected override IList<TImage> GetImages()
        {
            return RenderableImagesProvider.Images;
        }

        void INumberRenderer.Render(int value)
        {
            RenderDigits(value);
        }
    }
}