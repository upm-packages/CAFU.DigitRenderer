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

namespace CAFU.NumberRenderer
{
    public class RenderTest : ZenjectIntegrationTestFixture
    {
        [Inject] private INumberRenderer NumberRenderer { get; }

        private IList<(GameObject gameObject, Image component)> List { get; set; }

        [SetUp]
        public void Install()
        {
            PreInstall();
            List = InternalUtility.SetupRendererComponents<Image>(3);

            Container.BindInstance(new InitialState()).AsCached();
            Container.BindInterfacesTo<NumberRendererUseCase>().AsCached();
            Container.BindInterfacesTo<NumberRenderingPresenter<Image, Sprite>>().AsCached();
            Container.BindInterfacesTo<NumberRendererForUIImage>().FromNewComponentOnNewGameObject().AsCached();
            Container.Bind<IList<Image>>().FromInstance(List.Select(x => x.component).ToList()).AsCached();
            Container.Bind<IList<Sprite>>().FromInstance(InternalUtility.FindAssets<Sprite>(x => x.StartsWith("Assets/Tests/Runtime/Images")).ToList()).AsCached();
            Container.BindInstance(EmptyDigitType.None).AsCached();
            Container.Inject(this);

            PostInstall();
        }

        [Test]
        public void 描画できる()
        {
            NumberRenderer.Render(123);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("1"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("2"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("3"));
        }

        [Test]
        public void ゼロも描画できる()
        {
            NumberRenderer.Render(0);

            Assert.That(List[0].component.sprite, Is.Null);
            Assert.That(List[1].component.sprite, Is.Null);
            Assert.That(List[2].component.sprite.name, Is.EqualTo("0"));
        }
    }
}

