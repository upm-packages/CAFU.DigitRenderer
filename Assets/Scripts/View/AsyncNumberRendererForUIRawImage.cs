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
    public class AsyncNumberRendererForUIRawImage : AsyncNumberRendererBase, IAsyncGenericNumberRenderer<RawImage, Texture>
    {
        [Inject] IList<RawImage> IRendererComponentsProvider<RawImage>.Components { get; }
        [Inject] IList<AssetReferenceT<Texture>> IRenderableImagesProvider<AssetReferenceT<Texture>>.Images { get; }
    }
}