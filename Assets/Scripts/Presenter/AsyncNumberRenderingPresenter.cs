using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CAFU.NumberRenderer.Presenter
{
    internal class AsyncNumberRenderingPresenter<TComponent, TImage> :
        NumberRenderingPresenterBase<TComponent, TImage, AssetReferenceT<TImage>>,
        IAsyncNumberRenderer
        where TComponent : Component
        where TImage : Object
    {
        public AsyncNumberRenderingPresenter(IRendererComponentsProvider<TComponent> rendererComponentsProvider, IRenderableImagesProvider<AssetReferenceT<TImage>> renderableImagesProvider, IEmptyDigitsControllable emptyDigitsControllable) : base(rendererComponentsProvider, renderableImagesProvider, emptyDigitsControllable)
        {
            RenderableImagesProvider = renderableImagesProvider;
        }

        private IList<TImage> CachedImages { get; set; } = new List<TImage>();

        private IRenderableImagesProvider<AssetReferenceT<TImage>> RenderableImagesProvider { get; }

        protected override IList<TImage> GetImages()
        {
            return CachedImages;
        }

        async UniTask IAsyncNumberRenderer.RenderAsync(int value, CancellationToken cancellationToken)
        {
            if (!CachedImages.Any())
            {
                CachedImages = await UniTask
                    .WhenAll(
                        RenderableImagesProvider.Images.Select(
                            x => x
                                .LoadAssetAsync()
                                .WithCancellation(cancellationToken)
                        )
                    );
            }
            RenderDigits(value);
        }
    }
}