using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CAFU.NumberRenderer.Presenter
{
    internal interface IAsyncGenericNumberRenderer<TComponent, TImage> :
        IRendererComponentsProvider<TComponent>,
        IRenderableImagesProvider<AssetReferenceT<TImage>>
        where TComponent : Object
        where TImage : Object
    {
    }
}