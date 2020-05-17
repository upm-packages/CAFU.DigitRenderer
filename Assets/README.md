# CAFU NumberRenderer

## Installation

```bash
upm add package dev.upm-packages.cafu.numberrenderer
```

Note: `upm` command is provided by [this repository](https://github.com/upm-packages/upm-cli).

You can also edit `Packages/manifest.json` directly.

```jsonc
{
  "dependencies": {
    // (snip)
    "dev.upm-packages.cafu.numberrenderer": "[latest version]",
    // (snip)
  },
  "scopedRegistries": [
    {
      "name": "Unofficial Unity Package Manager Registry",
      "url": "https://upm-packages.dev",
      "scopes": [
        "dev.upm-packages"
      ]
    }
  ]
}
```

## Usages

### 1. Setup Renderer Components

Setup GameObjects and Components to render images.

![image](https://user-images.githubusercontent.com/838945/77058187-713ffe00-6a18-11ea-95e8-3e6d3c0114a1.png)

### 2. Setup Extenject Installer

Setup Installer.

![image](https://user-images.githubusercontent.com/838945/77059651-9cc3e800-6a1a-11ea-95c1-a3619e8d4a19.png)

1. Add `NumberRendererInstaller` to some Context.
    - This installer exposes `INumberRenderer` interface.
    - If you want to use `async` with `IAsyncNumberRenderer` interface, add `AsyncNumberRendererInstaller` instead of `NumberRendererInstaller`.
1. Choose `Renderer Type` to use to render all digits.
    - **`SpriteRenderer`**: Using `UnityEngine.SpriteRenderer`.
    - **`UIImage`**: Using `UnityEngine.UI.Image`.
    - **`UIRawImage`**: Using `UnityEngine.UI.RawImage`.
1. Put renderers into `Renderers` field.
    - Renderer components type must match to `Renderer Type`.
    - Please arrange the displayed numbers from left to right.
1. Put images into `Sprites` field or `Textures` field.
    - You must put `Sprites` if you choose `SpriteRenderer` or `UIImage` at `Renderer Type`.
    - You must put `Textures` if you choose `UITexture` at `Renderer Type`.
1. Choose `Empty Digit Type` to specify how to handle empty digits.
    - **`None`**: Nothing to do.
    - **`DestroyGameObject`**: Destroy GameObject related to empty image components.
    - **`DestroyImmediateGameObject`**: Destroy GameObject related to empty image components immediately.
    - **`DeactivateGameObject`**: Invoke `GameObject.SetActive(false)` on GameObject related to empty image components.
    - **`DestroyComponent`**: Destroy empty image components.
    - **`DestroyImmediateComponent`**: Destroy empty image components immediately.
    - **`DisableComponent`**: Set `false` to `Behaviour.enabled` of empty image components.
    - **`Transparent`**: Set `Color.clear` to `Material` or `Graphic.color` of empty image components.
    - **`ZeroFill`**: Render `0` image into empty image components.
1. Check `Should Render Initial Value` if you want to render initial value.
1. Input initial value to `Initial Value` field.

### 3. Render

Invoke `INumberRenderer.Render()` to render images.

```cs
using CAFU.NumberRenderer;

public class Foo
{
    public Foo(INumberRenderer numberRenderer)
    {
        NumberRenderer = numberRenderer;
    }

    private INumberRenderer NumberRenderer { get; }

    public void Run()
    {
        NumberRenderer.Render(123);
    }
}
```

You can also invoke asynchronous with `IAsyncNumberRenderer.RenderAsync()` to use `Addressable.AssetReferenceSprite` or `Addressable.AssetReferenceTexture` instead of to use `Sprite` or `Texture` directly.

```cs
using CAFU.NumberRenderer;
using UniRx.Async;

public class Bar
{
    public Bar(IAsyncNumberRenderer asyncNumberRenderer)
    {
        AsyncNumberRenderer = asyncNumberRenderer;
    }

    private IAsyncNumberRenderer AsyncNumberRenderer { get; }

    public async UniTask RunAsync(CancellationToken cancellationToken = default)
    {
        await AsyncNumberRenderer.RenderAsync(123, cancellationToken);
    }
}
```
