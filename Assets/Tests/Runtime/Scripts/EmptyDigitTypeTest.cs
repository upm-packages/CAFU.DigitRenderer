using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CAFU.NumberRenderer.Entity;
using CAFU.NumberRenderer.Presenter;
using CAFU.NumberRenderer.UseCase;
using CAFU.NumberRenderer.View;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer
{
    public class EmptyDigitTypeTest : ZenjectIntegrationTestFixture
    {
        [Inject] private INumberRenderer NumberRenderer { get; }

        [SetUp]
        public void Install()
        {
            PreInstall();

            Container.BindInstance(new InitialState()).AsCached();
            Container.BindInterfacesTo<NumberRendererUseCase>().AsCached();
            Container.BindInterfacesTo<NumberRenderingPresenter<Image, Sprite>>().AsCached();
            Container.Bind<IList<Sprite>>().FromInstance(InternalUtility.FindAssets<Sprite>(x => x.StartsWith("Assets/Tests/Runtime/Images")).ToList()).AsCached();
            Container.BindInterfacesTo<NumberRendererForUIImage>().FromNewComponentOnNewGameObject().AsCached();
        }

        [Test]
        public void 不要桁_何もしない()
        {
            var list = Prepare<Image>(EmptyDigitType.None);

            NumberRenderer.Render(45);

            Assert.That(list[0].component.sprite, Is.Null);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [UnityTest]
        public IEnumerator 不要桁_GameObjectを破棄()
        {
            var list = Prepare<Image>(EmptyDigitType.DestroyGameObject);

            NumberRenderer.Render(45);

            // 1フレーム待たないと Destroy が効かない
            yield return null;

            Assert.That(list[0].component == null, Is.True);
            Assert.That(list[0].gameObject == null, Is.True);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [Test]
        public void 不要桁_GameObjectを即時破棄()
        {
            var list = Prepare<Image>(EmptyDigitType.DestroyImmediateGameObject);

            NumberRenderer.Render(45);

            Assert.That(list[0].component == null, Is.True);
            Assert.That(list[0].gameObject == null, Is.True);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [Test]
        public void 不要桁_GameObjectを非アクティブ()
        {
            var list = Prepare<Image>(EmptyDigitType.DeactivateGameObject);

            NumberRenderer.Render(45);

            Assert.That(list[0].component == null, Is.False);
            Assert.That(list[0].gameObject == null, Is.False);
            Assert.That(list[0].component.enabled, Is.True);
            Assert.That(list[0].gameObject.activeSelf, Is.False);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [UnityTest]
        public IEnumerator 不要桁_Componentを破棄()
        {
            var list = Prepare<Image>(EmptyDigitType.DestroyComponent);

            NumberRenderer.Render(45);

            // 1フレーム待たないと Destroy が効かない
            yield return null;

            Assert.That(list[0].component == null, Is.True);
            Assert.That(list[0].gameObject == null, Is.False);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [Test]
        public void 不要桁_Componentを即時破棄()
        {
            var list = Prepare<Image>(EmptyDigitType.DestroyImmediateComponent);

            NumberRenderer.Render(45);

            Assert.That(list[0].component == null, Is.True);
            Assert.That(list[0].gameObject == null, Is.False);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [Test]
        public void 不要桁_Componentを無効化()
        {
            var list = Prepare<Image>(EmptyDigitType.DisableComponent);

            NumberRenderer.Render(45);

            Assert.That(list[0].component == null, Is.False);
            Assert.That(list[0].gameObject == null, Is.False);
            Assert.That(list[0].component.enabled, Is.False);
            Assert.That(list[0].gameObject.activeSelf, Is.True);
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [Test]
        public void 不要桁_透明化()
        {
            var list = Prepare<Image>(EmptyDigitType.Transparent);

            NumberRenderer.Render(45);

            Assert.That(list[0].component == null, Is.False);
            Assert.That(list[0].gameObject == null, Is.False);
            Assert.That(list[0].component.enabled, Is.True);
            Assert.That(list[0].gameObject.activeSelf, Is.True);
            Assert.That(list[0].component.color, Is.EqualTo(Color.clear));
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        [Test]
        public void 不要桁_ゼロ埋め()
        {
            var list = Prepare<Image>(EmptyDigitType.ZeroFill);

            NumberRenderer.Render(45);

            Assert.That(list[0].component == null, Is.False);
            Assert.That(list[0].gameObject == null, Is.False);
            Assert.That(list[0].component.enabled, Is.True);
            Assert.That(list[0].gameObject.activeSelf, Is.True);
            Assert.That(list[0].component.color, Is.EqualTo(Color.white));
            Assert.That(list[0].component.sprite.name, Is.EqualTo("0"));
            Assert.That(list[1].component.sprite.name, Is.EqualTo("4"));
            Assert.That(list[2].component.sprite.name, Is.EqualTo("5"));
        }

        private IList<(GameObject gameObject, TComponent component)> Prepare<TComponent>(EmptyDigitType emptyDigitType) where TComponent : Component
        {
            var list = InternalUtility.SetupRendererComponents<TComponent>(3);
            Container.BindInstance(emptyDigitType).AsCached();
            Container.Bind<IList<TComponent>>().FromInstance(list.Select(x => x.component).ToList()).AsCached();
            Container.Inject(this);
            // ココで実行しないと EmptyDigitType の Bind が効かない
            PostInstall();
            return list;
        }
    }
}