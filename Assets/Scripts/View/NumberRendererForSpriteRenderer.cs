using System.Collections.Generic;
using CAFU.NumberRenderer.Internal;
using CAFU.NumberRenderer.Presenter;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace CAFU.NumberRenderer.View
{
    [UsedImplicitly]
    public class NumberRendererForSpriteRenderer : NumberRendererBase, IGenericNumberRenderer<SpriteRenderer, Sprite>
    {
        [Inject] IList<SpriteRenderer> IRendererComponentsProvider<SpriteRenderer>.Components { get; }
        [Inject] IList<Sprite> IRenderableImagesProvider<Sprite>.Images { get; }
    }
}