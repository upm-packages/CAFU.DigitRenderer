using System.Collections.Generic;
using CAFU.NumberRenderer.Internal;
using CAFU.NumberRenderer.Presenter;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer.View
{
    [UsedImplicitly]
    public class AsyncNumberRendererForUIImage : AsyncNumberRendererBase, IAsyncGenericNumberRenderer<Image, Sprite>
    {
        [Inject] IList<Image> IRendererComponentsProvider<Image>.Components { get; }
        [Inject] IList<AssetReferenceT<Sprite>> IRenderableImagesProvider<AssetReferenceT<Sprite>>.Images { get; }
    }
}