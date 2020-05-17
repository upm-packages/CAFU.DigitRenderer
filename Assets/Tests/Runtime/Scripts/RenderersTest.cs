using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.NumberRenderer.Entity;
using CAFU.NumberRenderer.Presenter;
using CAFU.NumberRenderer.UseCase;
using CAFU.NumberRenderer.View;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace CAFU.NumberRenderer
{
    public class RenderersTest : ZenjectIntegrationTestFixture
    {
        [Inject] private INumberRenderer NumberRenderer { get; }

        [SetUp]
        public void Install()
        {
            PreInstall();

            Container.BindInstance(new InitialState()).AsCached();
            Container.BindInterfacesTo<NumberRendererUseCase>().AsCached();
            Container.Bind<IList<Sprite>>().FromInstance(InternalUtility.FindAssets<Sprite>(x => x.StartsWith("Assets/Tests/Runtime/Images")).ToList()).AsCached();
            Container.Bind<IList<Texture>>().FromInstance(InternalUtility.FindAssets<Texture>(x => x.StartsWith("Assets/Tests/Runtime/Images")).ToList()).AsCached();
            Container.BindInstance(EmptyDigitType.None).AsCached();
        }

        [Test]
        public void SpriteRendererで描画できる()
        {
            var list = Prepare<SpriteRenderer, Sprite>(RendererType.SpriteRenderer);
            NumberRenderer.Render(123);

            Assert.That(list[0].component.sprite.name, Is.EqualTo("1"));
            Assert.That(list[1].component.sprite.name, Is.EqualTo("2"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("3"));
        }

        [Test]
        public void UIImageで描画できる()
        {
            var list = Prepare<Image, Sprite>(RendererType.UIImage);
            NumberRenderer.Render(123);

            Assert.That(list[0].component.sprite.name, Is.EqualTo("1"));
            Assert.That(list[1].component.sprite.name, Is.EqualTo("2"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("3"));
        }

        [Test]
        public void UIRawImageで描画できる()
        {
            var list = Prepare<RawImage, Texture>(RendererType.UIRawImage);
            NumberRenderer.Render(123);

            Assert.That(list[0].component.texture.name, Is.EqualTo("1"));
            Assert.That(list[1].component.texture.name, Is.EqualTo("2"));
            Assert.That(list[2].component.texture.name, Is.EqualTo("3"));
        }

        private IList<(GameObject gameObject, TComponent component)> Prepare<TComponent, TImage>(RendererType rendererType)
            where TComponent : Component
            where TImage : Object
        {
            var list = InternalUtility.SetupRendererComponents<TComponent>(3);
            Container.Bind<INumberRenderer>().To<NumberRenderingPresenter<TComponent, TImage>>().AsCached();
            Container.Bind<IList<TComponent>>().FromInstance(list.Select(x => x.component).ToList()).AsCached();
            switch (rendererType)
            {
                case RendererType.SpriteRenderer:
                    Container.BindInterfacesTo<NumberRendererForSpriteRenderer>().FromNewComponentOnNewGameObject().AsCached();
                    break;
                case RendererType.UIImage:
                    Container.BindInterfacesTo<NumberRendererForUIImage>().FromNewComponentOnNewGameObject().AsCached();
                    break;
                case RendererType.UIRawImage:
                    Container.BindInterfacesTo<NumberRendererForUIRawImage>().FromNewComponentOnNewGameObject().AsCached();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rendererType), rendererType, null);
            }

            Container.Inject(this);
            // ココで実行しないと IList<TComponent> の Bind が効かない
            PostInstall();
            return list;
        }
    }
}