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
    public class NumberRendererForUIImage : NumberRendererBase, IGenericNumberRenderer<Image, Sprite>
    {
        [Inject] IList<Image> IRendererComponentsProvider<Image>.Components { get; }
        [Inject] IList<Sprite> IRenderableImagesProvider<Sprite>.Images { get; }
    }
}