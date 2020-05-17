using System.Collections.Generic;
using CAFU.NumberRenderer.Internal;
using CAFU.NumberRenderer.Presenter;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer.View
{
    [UsedImplicitly]
    public class NumberRendererForUIRawImage : NumberRendererBase, IGenericNumberRenderer<RawImage, Texture>
    {
        [Inject] IList<RawImage> IRendererComponentsProvider<RawImage>.Components { get; }
        [Inject] IList<Texture> IRenderableImagesProvider<Texture>.Images { get; }
    }
}