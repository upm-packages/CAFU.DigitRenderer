using System.Collections.Generic;
using UnityEngine;

namespace CAFU.NumberRenderer.Presenter
{
    internal interface IRendererComponentsProvider<TComponent> where TComponent : Object
    {
        IList<TComponent> Components { get; }
    }
}