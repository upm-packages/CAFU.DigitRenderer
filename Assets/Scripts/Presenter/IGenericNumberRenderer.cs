using UnityEngine;

namespace CAFU.NumberRenderer.Presenter
{
    internal interface IGenericNumberRenderer<TComponent, TImage> :
        IRendererComponentsProvider<TComponent>,
        IRenderableImagesProvider<TImage>
        where TComponent : Object
        where TImage : Object
    {
    }
}