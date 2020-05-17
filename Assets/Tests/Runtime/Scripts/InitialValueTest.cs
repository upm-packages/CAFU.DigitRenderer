using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CAFU.NumberRenderer.Controller;
using CAFU.NumberRenderer.Entity;
using CAFU.NumberRenderer.Presenter;
using CAFU.NumberRenderer.UseCase;
using CAFU.NumberRenderer.View;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer
{
    public class InitialValueTest : ZenjectIntegrationTestFixture
    {
        [InjectOptional] private INumberRenderer NumberRenderer { get; }
        [InjectOptional] private IAsyncNumberRenderer AsyncNumberRenderer { get; }

        private IList<(GameObject gameObject, Image component)> List { get; set; }

        [SetUp]
        public void Install()
        {
            PreInstall();
            List = InternalUtility.SetupRendererComponents<Image>(3);

            Container.Bind<IList<Image>>().FromInstance(List.Select(x => x.component).ToList()).AsCached();
            Container.BindInstance(EmptyDigitType.None).AsCached();
        }

        [UnityTest]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerator 同期_初期値なし() => UniTask.ToCoroutine(async () =>
        {
            Prepare(false, false);

            await UniTask.DelayFrame(1);

            Assert.That(List[0].component.sprite, Is.Null);
            Assert.That(List[1].component.sprite, Is.Null);
            Assert.That(List[2].component.sprite, Is.Null);

            NumberRenderer.Render(123);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("1"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("2"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("3"));
        });

        [UnityTest]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerator 同期_初期値あり() => UniTask.ToCoroutine(async () =>
        {
            Prepare(false, true);

            await UniTask.DelayFrame(1);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("7"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("8"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("9"));

            NumberRenderer.Render(123);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("1"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("2"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("3"));
        });

        [UnityTest]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerator 非同期_初期値なし() => UniTask.ToCoroutine(async () =>
        {
            Prepare(true, false);

            // 描画完了を通知する手段がないため、雑に待つ
            await UniTask.DelayFrame(10);

            Assert.That(List[0].component.sprite, Is.Null);
            Assert.That(List[1].component.sprite, Is.Null);
            Assert.That(List[2].component.sprite, Is.Null);

            await AsyncNumberRenderer.RenderAsync(456);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("4"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("5"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("6"));
        });

        [UnityTest]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerator 非同期_初期値あり() => UniTask.ToCoroutine(async () =>
        {
            Prepare(true, true);

            // 描画完了を通知する手段がないため、雑に待つ
            await UniTask.DelayFrame(10);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("7"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("8"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("9"));

            await AsyncNumberRenderer.RenderAsync(456);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("4"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("5"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("6"));
        });

        private void Prepare(bool useAsync, bool shouldRenderInitialValue)
        {
            Container.BindInterfacesTo<InitializationController>().AsCached();
            if (useAsync)
            {
                Container.BindInterfacesTo<AsyncNumberRendererUseCase>().AsCached();
                Container.BindInterfacesTo<AsyncNumberRenderingPresenter<Image, Sprite>>().AsCached();
                Container.BindInterfacesTo<AsyncNumberRendererForUIImage>().FromNewComponentOnNewGameObject().AsCached();
                Container
                    // Renderer 側で受け付ける型は AssetReferenceT<TObject> 型
                    .Bind<IList<AssetReferenceT<Sprite>>>()
                    .FromInstance(
                        InternalUtility
                            .FindGUIDs<Sprite>(x => x.StartsWith("Assets/Tests/Runtime/Images"))
                            // AssetReference のコンストラクタに GUID を渡すコトでインスタンスを作れる
                            .Select(x => new AssetReferenceT<Sprite>(x))
                            .ToList()
                    )
                    .AsCached();
            }
            else
            {
                Container.BindInterfacesTo<NumberRendererUseCase>().AsCached();
                Container.BindInterfacesTo<NumberRenderingPresenter<Image, Sprite>>().AsCached();
                Container.BindInterfacesTo<NumberRendererForUIImage>().FromNewComponentOnNewGameObject().AsCached();
                Container.Bind<IList<Sprite>>().FromInstance(InternalUtility.FindAssets<Sprite>(x => x.StartsWith("Assets/Tests/Runtime/Images")).ToList()).AsCached();
            }

            Container.BindInstance(new InitialState(shouldRenderInitialValue, 789)).AsCached();

            Container.Inject(this);

            PostInstall();
        }
    }
}