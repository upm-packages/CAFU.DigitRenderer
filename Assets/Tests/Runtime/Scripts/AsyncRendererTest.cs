using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CAFU.NumberRenderer.Entity;
using CAFU.NumberRenderer.Presenter;
using CAFU.NumberRenderer.UseCase;
using CAFU.NumberRenderer.View;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;

namespace CAFU.NumberRenderer
{
    public class AsyncRendererTest : ZenjectIntegrationTestFixture
    {
        [Inject] private IAsyncNumberRenderer AsyncNumberRenderer { get; }

        private IList<(GameObject gameObject, Image component)> List { get; set; }

        [SetUp]
        public void Install()
        {
            PreInstall();
            List = InternalUtility.SetupRendererComponents<Image>(3);

            Container.BindInstance(new InitialState()).AsCached();
            Container.BindInterfacesTo<AsyncNumberRendererUseCase>().AsCached();
            Container.BindInterfacesTo<AsyncNumberRenderingPresenter<Image, Sprite>>().AsCached();
            Container.BindInterfacesTo<AsyncNumberRendererForUIImage>().FromNewComponentOnNewGameObject().AsCached();
            Container.Bind<IList<Image>>().FromInstance(List.Select(x => x.component).ToList()).AsCached();
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
            Container.BindInstance(EmptyDigitType.None).AsCached();
            Container.Inject(this);

            PostInstall();
        }

        [UnityTest]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerator 非同期読み込みできる() => UniTask.ToCoroutine(async () =>
        {
            Assert.That(List[0].component.sprite, Is.Null);
            Assert.That(List[1].component.sprite, Is.Null);
            Assert.That(List[2].component.sprite, Is.Null);

            await AsyncNumberRenderer.RenderAsync(456);

            Assert.That(List[0].component.sprite.name, Is.EqualTo("4"));
            Assert.That(List[1].component.sprite.name, Is.EqualTo("5"));
            Assert.That(List[2].component.sprite.name, Is.EqualTo("6"));
        });
    }
}