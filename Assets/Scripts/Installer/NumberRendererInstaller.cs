using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.NumberRenderer.Controller;
using CAFU.NumberRenderer.Entity;
using CAFU.NumberRenderer.Presenter;
using CAFU.NumberRenderer.UseCase;
using CAFU.NumberRenderer.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer.Installer
{
    public class NumberRendererInstaller : MonoInstaller<NumberRendererInstaller>
    {
        [SerializeField] private RendererType rendererType = default;
        [SerializeField] private List<GameObject> renderers = default;
        [SerializeField] private List<Sprite> sprites = default;
        [SerializeField] private List<Texture> textures = default;
        [SerializeField] private EmptyDigitType emptyDigitType = default;
        [SerializeField] private bool shouldRenderInitialValue = true;
        [SerializeField] private int initialValue = default;
        private RendererType RendererType => rendererType;
        private IEnumerable<GameObject> Renderers => renderers;
        private IList<Sprite> Sprites => sprites;
        private IList<Texture> Textures => textures;
        private EmptyDigitType EmptyDigitType => emptyDigitType;
        private bool ShouldRenderInitialValue => shouldRenderInitialValue;
        private int InitialValue => initialValue;

        [UsedImplicitly] public BindingCondition BindingCondition { get; set; } = _ => true;

        public override void InstallBindings()
        {
            Container
                .Bind<INumberRenderer>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .WithGameObjectName(nameof(NumberRendererInstaller))
                .AsCached()
                .When(BindingCondition)
                .NonLazy();
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer.BindInstance(new InitialState(ShouldRenderInitialValue, InitialValue)).AsCached();
            subContainer.BindInterfacesTo<NumberRendererUseCase>().AsCached();
            subContainer.BindInterfacesTo<InitializationController>().AsCached();
            switch (RendererType)
            {
                case RendererType.SpriteRenderer:
                    subContainer.BindInterfacesTo<NumberRenderingPresenter<SpriteRenderer, Sprite>>().AsCached();
                    subContainer.BindInterfacesTo<NumberRendererForSpriteRenderer>().FromNewComponentOn(gameObject).AsCached();
                    subContainer.Bind<IList<SpriteRenderer>>().FromInstance(Renderers.Select(x => x.GetComponent<SpriteRenderer>()).ToList()).AsCached();
                    subContainer.BindInstance(Sprites).AsCached();
                    break;
                case RendererType.UIImage:
                    subContainer.BindInterfacesTo<NumberRenderingPresenter<Image, Sprite>>().AsCached();
                    subContainer.BindInterfacesTo<NumberRendererForUIImage>().FromNewComponentOn(gameObject).AsCached();
                    subContainer.Bind<IList<Image>>().FromInstance(Renderers.Select(x => x.GetComponent<Image>()).ToList()).AsCached();
                    subContainer.BindInstance(Sprites).AsCached();
                    break;
                case RendererType.UIRawImage:
                    subContainer.BindInterfacesTo<NumberRenderingPresenter<RawImage, Texture>>().AsCached();
                    subContainer.BindInterfacesTo<NumberRendererForUIRawImage>().FromNewComponentOn(gameObject).AsCached();
                    subContainer.Bind<IList<RawImage>>().FromInstance(Renderers.Select(x => x.GetComponent<RawImage>()).ToList()).AsCached();
                    subContainer.BindInstance(Textures).AsCached();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            subContainer.BindInstance(EmptyDigitType).AsCached();
            subContainer.BindInstance(ShouldRenderInitialValue).AsCached();
            subContainer.BindInstance(InitialValue).AsCached();
        }
    }
}