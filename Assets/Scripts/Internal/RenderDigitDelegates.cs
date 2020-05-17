using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CAFU.NumberRenderer.Internal
{
    internal static class RenderDigitDelegates
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Setup()
        {
            RenderDigitDelegate<SpriteRenderer, Sprite>.TransparentDelegate = (component, image) => component.material.color = Color.clear;
            RenderDigitDelegate<RawImage, Texture>.TransparentDelegate = (component, image) => component.color = Color.clear;
            RenderDigitDelegate<Image, Sprite>.TransparentDelegate = (component, image) => component.color = Color.clear;

            RenderDigitDelegate<SpriteRenderer, Sprite>.SetImageDelegate = (component, image) => component.sprite = image;
            RenderDigitDelegate<RawImage, Texture>.SetImageDelegate = (component, image) => component.texture = image;
            RenderDigitDelegate<Image, Sprite>.SetImageDelegate = (component, image) => component.sprite = image;

            RenderDigitDelegate<SpriteRenderer, Sprite>.SetupDelegateMap();
            RenderDigitDelegate<RawImage, Texture>.SetupDelegateMap();
            RenderDigitDelegate<Image, Sprite>.SetupDelegateMap();
        }
    }

    internal static class RenderDigitDelegate<TComponent, TImage>
        where TComponent : Component
        where TImage : Object
    {
        internal static Action<TComponent, TImage> TransparentDelegate { get; set; }
        internal static Action<TComponent, TImage> SetImageDelegate { get; set; }

        internal static IDictionary<EmptyDigitType, Action<TComponent, TImage>> EmptyDigitDelegateMap { get; private set; }

        internal static void SetupDelegateMap()
        {
            EmptyDigitDelegateMap = new Dictionary<EmptyDigitType, Action<TComponent, TImage>>
            {
                {EmptyDigitType.DestroyGameObject, DestroyGameObject},
                {EmptyDigitType.DestroyImmediateGameObject, DestroyImmediateGameObject},
                {EmptyDigitType.DeactivateGameObject, DeactivateGameObject},
                {EmptyDigitType.DestroyComponent, DestroyComponent},
                {EmptyDigitType.DestroyImmediateComponent, DestroyImmediateComponent},
                {EmptyDigitType.DisableComponent, DisableComponent},
                {EmptyDigitType.Transparent, TransparentDelegate},
                {EmptyDigitType.ZeroFill, SetImageDelegate},
            };
        }

        private static void DestroyGameObject(Component component, Object image)
        {
            Object.Destroy(component.gameObject);
        }

        private static void DestroyImmediateGameObject(Component component, Object image)
        {
            Object.DestroyImmediate(component.gameObject);
        }

        private static void DeactivateGameObject(Component component, Object image)
        {
            component.gameObject.SetActive(false);
        }

        private static void DestroyComponent(Component component, Object image)
        {
            Object.Destroy(component);
        }

        private static void DestroyImmediateComponent(Component component, Object image)
        {
            Object.DestroyImmediate(component);
        }

        private static void DisableComponent(Component component, Object image)
        {
            if (component is Behaviour behaviour)
            {
                behaviour.enabled = false;
            }
        }
    }
}