using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.NumberRenderer.Controller;
using CAFU.NumberRenderer.Entity;
using CAFU.NumberRenderer.Presenter;
using CAFU.NumberRenderer.UseCase;
using CAFU.NumberRenderer.View;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer.Installer
{
    public class AsyncNumberRendererInstaller : MonoInstaller<AsyncNumberRendererInstaller>
    {
        [SerializeField] private RendererType rendererType = default;
        [SerializeField] private List<GameObject> renderers = default;
        [SerializeField] private List<AssetReferenceSprite> assetReferenceSprites = default;
        [SerializeField] private List<AssetReferenceTexture> assetReferenceTextures = default;
        [SerializeField] private EmptyDigitType emptyDigitType = default;
        [SerializeField] private bool shouldRenderInitialValue = true;
        [SerializeField] private int initialValue = default;
        private RendererType RendererType => rendererType;
        private IEnumerable<GameObject> Renderers => renderers;
        private IEnumerable<AssetReferenceSprite> AssetReferenceSprites => assetReferenceSprites;
        private IEnumerable<AssetReferenceTexture> AssetReferenceTextures => assetReferenceTextures;
        private EmptyDigitType EmptyDigitType => emptyDigitType;
        private bool ShouldRenderInitialValue => shouldRenderInitialValue;
        private int InitialValue => initialValue;

        public override void InstallBindings()
        {
            Container
                .Bind<IAsyncNumberRenderer>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .WithGameObjectName(nameof(NumberRendererInstaller))
                .AsCached()
                .NonLazy();
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer.BindInstance(new InitialState(ShouldRenderInitialValue, InitialValue)).AsCached();
            subContainer.BindInterfacesTo<AsyncNumberRendererUseCase>().AsCached();
            subContainer.BindInterfacesTo<InitializationController>().AsCached();
            switch (RendererType)
            {
                case RendererType.SpriteRenderer:
                    subContainer.BindInterfacesTo<AsyncNumberRenderingPresenter<SpriteRenderer, Sprite>>().AsCached();
                    subContainer.BindInterfacesTo<AsyncNumberRendererForSpriteRenderer>().FromNewComponentOn(gameObject).AsCached();
                    subContainer.Bind<IList<SpriteRenderer>>().FromInstance(Renderers.Select(x => x.GetComponent<SpriteRenderer>()).ToList()).AsCached();
                    subContainer.Bind<IList<AssetReferenceT<Sprite>>>().FromInstance(AssetReferenceSprites.OfType<AssetReferenceT<Sprite>>().ToList()).AsCached();
                    break;
                case RendererType.UIImage:
                    subContainer.BindInterfacesTo<AsyncNumberRenderingPresenter<Image, Sprite>>().AsCached();
                    subContainer.BindInterfacesTo<AsyncNumberRendererForUIImage>().FromNewComponentOn(gameObject).AsCached();
                    subContainer.Bind<IList<Image>>().FromInstance(Renderers.Select(x => x.GetComponent<Image>()).ToList()).AsCached();
                    subContainer.Bind<IList<AssetReferenceT<Sprite>>>().FromInstance(AssetReferenceSprites.OfType<AssetReferenceT<Sprite>>().ToList()).AsCached();
                    break;
                case RendererType.UIRawImage:
                    subContainer.BindInterfacesTo<AsyncNumberRenderingPresenter<RawImage, Texture>>().AsCached();
                    subContainer.BindInterfacesTo<AsyncNumberRendererForUIRawImage>().FromNewComponentOn(gameObject).AsCached();
                    subContainer.Bind<IList<RawImage>>().FromInstance(Renderers.Select(x => x.GetComponent<RawImage>()).ToList()).AsCached();
                    subContainer.Bind<IList<AssetReferenceT<Texture>>>().FromInstance(AssetReferenceTextures.OfType<AssetReferenceT<Texture>>().ToList()).AsCached();
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