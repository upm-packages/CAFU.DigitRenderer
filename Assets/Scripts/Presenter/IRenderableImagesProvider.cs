using System.Collections.Generic;

namespace CAFU.NumberRenderer.Presenter
{
    internal interface IRenderableImagesProvider<TImage>
    {
        IList<TImage> Images { get; }
    }
}