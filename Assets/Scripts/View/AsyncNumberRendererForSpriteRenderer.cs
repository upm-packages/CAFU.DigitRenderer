using System.Collections.Generic;
using CAFU.NumberRenderer.Internal;
using CAFU.NumberRenderer.Presenter;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CAFU.NumberRenderer.View
{
    [UsedImplicitly]
    public class AsyncNumberRendererForSpriteRenderer : AsyncNumberRendererBase, IAsyncGenericNumberRenderer<SpriteRenderer, Sprite>
    {
        [Inject] IList<SpriteRenderer> IRendererComponentsProvider<SpriteRenderer>.Components { get; }
        [Inject] IList<AssetReferenceT<Sprite>> IRenderableImagesProvider<AssetReferenceT<Sprite>>.Images { get; }
    }
}