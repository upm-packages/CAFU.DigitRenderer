using CAFU.NumberRenderer.Presenter;
using UnityEngine;
using Zenject;

namespace CAFU.NumberRenderer.View
{
    public abstract class AsyncNumberRendererBase : MonoBehaviour, IEmptyDigitsControllable
    {
        [Inject] EmptyDigitType IEmptyDigitsControllable.EmptyDigitType { get; }
    }
}