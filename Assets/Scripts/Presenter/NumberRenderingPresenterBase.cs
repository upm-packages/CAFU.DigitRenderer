using System;
using System.Collections.Generic;
using System.Linq;
using CAFU.NumberRenderer.Internal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CAFU.NumberRenderer.Presenter
{
    internal abstract class NumberRenderingPresenterBase<TComponent, TImage> : NumberRenderingPresenterBase<TComponent, TImage, TImage>
        where TComponent : Component
        where TImage : Object
    {
        protected NumberRenderingPresenterBase(IRendererComponentsProvider<TComponent> rendererComponentsProvider, IRenderableImagesProvider<TImage> renderableImagesProvider, IEmptyDigitsControllable emptyDigitsControllable) : base(rendererComponentsProvider, renderableImagesProvider, emptyDigitsControllable)
        {
        }
    }

    internal abstract class NumberRenderingPresenterBase<TComponent, TImage, TRenderableImage>
        where TComponent : Component
        where TImage : Object
    {
        protected NumberRenderingPresenterBase(IRendererComponentsProvider<TComponent> rendererComponentsProvider, IRenderableImagesProvider<TRenderableImage> renderableImagesProvider, IEmptyDigitsControllable emptyDigitsControllable)
        {
            RendererComponentsProvider = rendererComponentsProvider;
            RenderableImagesProvider = renderableImagesProvider;
            EmptyDigitsControllable = emptyDigitsControllable;
        }

        private IRendererComponentsProvider<TComponent> RendererComponentsProvider { get; }
        private IRenderableImagesProvider<TRenderableImage> RenderableImagesProvider { get; }
        private IEmptyDigitsControllable EmptyDigitsControllable { get; }

        protected abstract IList<TImage> GetImages();

        protected void RenderDigits(int value)
        {
            Validate(value);

            var components = RendererComponentsProvider.Components;
            var images = GetImages();

            InternalUtility.SplitDigits(value)
                .FillEmpty(components.Count)
                .Select((digit, index) => (digit, index))
                .Where(x => components.Count > x.index && images.Count > x.digit)
                .Select(
                    x => (
                        // Get component from the end of list
                        component: components[components.Count - x.index - 1],
                        // `digit' will be a negative value if empty digit
                        image: x.digit >= 0 ?
                            images[x.digit] :
                            // Use 0 image if `emptyDigitType' is specified ZeroFill
                            EmptyDigitsControllable.EmptyDigitType == EmptyDigitType.ZeroFill ?
                                images[0] :
                                default
                    )
                )
                .ToList()
                .ForEach(RenderDigitImage);
        }

        private void Validate(int value)
        {
            if (!RendererComponentsProvider.Components.Any())
            {
                throw new ArgumentException("No components were found.");
            }

            if (!RenderableImagesProvider.Images.Any())
            {
                throw new ArgumentException("No images were found.");
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Cannot render negative value: [value: {value}]");
            }

            if (value >= Math.Pow(10, RendererComponentsProvider.Components.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Cannot render value greater than digits length: [value: {value}, digits length: {RendererComponentsProvider.Components.Count}, max value: {Math.Pow(10, RendererComponentsProvider.Components.Count) - 1}]");
            }
        }

        private void RenderDigitImage((TComponent component, TImage image) item)
        {
            if (item.image == default)
            {
                if (EmptyDigitsControllable.EmptyDigitType == EmptyDigitType.None)
                {
                    return;
                }

                RenderDigitDelegate<TComponent, TImage>.EmptyDigitDelegateMap[EmptyDigitsControllable.EmptyDigitType](item.component, item.image);
            }
            else
            {
                RenderDigitDelegate<TComponent, TImage>.SetImageDelegate(item.component, item.image);
            }
        }
    }
}