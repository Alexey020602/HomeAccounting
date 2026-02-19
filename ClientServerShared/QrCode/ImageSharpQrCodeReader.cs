using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing;

namespace ClientServerShared.QrCode;

/// <summary>
/// Service for reading barcodes from images using ImageSharp library.
/// </summary>
public class ImageSharpQrCodeReader(IBarcodeReader<Image<L8>> reader) : IQrCodeReader
{
    // Константы для размеров изображений
    private const int LargeImageThreshold = 1500;
    private const int VeryLargeImageThreshold = 3000;
    private const int SmallImageThreshold = 2000;
    private const int CroppedRegionThreshold = 800;
    
    // Константы для обработки изображений
    private static readonly int[] ResizeSizes = [1200, 1000, 800];
    private static readonly float[] BinaryThresholds = [0.35f, 0.38f, 0.4f, 0.42f, 0.45f, 0.48f, 0.5f, 0.52f, 0.55f, 0.58f, 0.6f, 0.62f, 0.65f
    ];
    private static readonly float[] CroppedBinaryThresholds = [0.4f, 0.45f, 0.5f, 0.55f, 0.6f];
    private static readonly float[] BlurValues = [0.8f, 1.2f, 1.5f, 2.0f];
    private static readonly float[] ContrastValues = [1.5f, 2.0f, 2.5f];
    private static readonly float[] ThresholdValues = [0.45f, 0.5f, 0.55f];
    
    // Параметры обрезки: (widthFactor, heightFactor, xOffset, yOffset)
    private static readonly (float WidthFactor, float HeightFactor, float XOffset, float YOffset)[] CropRegionParameters =
    [
        (0.9f, 0.7f, 0.05f, 0.15f), // Центральная область
        (0.9f, 0.5f, 0.05f, 0.0f),  // Верхняя центральная
        (0.9f, 0.5f, 0.05f, 0.5f),  // Нижняя центральная
        (0.5f, 0.9f, 0.0f, 0.05f),  // Левая центральная
        (0.5f, 0.9f, 0.5f, 0.05f),  // Правая центральная
    ];

    /// <summary>
    /// Reads barcode from stream using ImageSharp.
    /// </summary>
    public async ValueTask<string> ReadQrCodeAsync(Stream stream)
    {
        using var rgba = await Image.LoadAsync<Rgba32>(stream);
        rgba.Mutate(x => x.AutoOrient());
        using var baseL8 = rgba.CloneAs<L8>();

        foreach (var variant in BuildVariants(baseL8))
        {
            using (variant)
            {
                var result = reader.Decode(variant);
                if (result?.Text is { Length: > 0 } text)
                    return text;
            }
        }

        throw new QrCodeReaderException("Cannot decode image");
    }

    private static IEnumerable<Image<L8>> BuildVariants(Image<L8> image)
    {
        var maxDimension = Math.Max(image.Width, image.Height);
        using var workingImage = PrepareWorkingImage(image, maxDimension);

        // Базовые варианты: оригинал и разные размеры
        foreach (var variant in GetBasicVariants(workingImage, maxDimension))
            yield return variant;

        // Базовые варианты обработки
        foreach (var variant in GetBasicProcessingVariants(workingImage))
            yield return variant;

        // Агрессивная обработка для выцветших кодов
        foreach (var variant in GetAggressiveProcessingVariants(workingImage))
            yield return variant;

        // Инверсия для темных QR-кодов на светлом фоне
        foreach (var variant in GetInvertedVariants(workingImage))
            yield return variant;

        // Пороговая бинаризация
        foreach (var variant in GetBinaryThresholdVariants(workingImage))
            yield return variant;

        // Комбинации контраста и яркости
        foreach (var variant in GetContrastBrightnessVariants(workingImage))
            yield return variant;

        // Комплексная обработка для выцветших кодов
        foreach (var variant in GetFadedCodeVariants(workingImage))
            yield return variant;

        // Агрессивная инверсия
        foreach (var variant in GetAggressiveInvertedVariants(workingImage))
            yield return variant;

        // Множественные проходы размытия
        foreach (var variant in GetDoubleBlurVariants(workingImage))
            yield return variant;

        // Обрезанные области
        foreach (var variant in GetCroppedRegionVariants(workingImage))
            yield return variant;
    }

    /// <summary>
    /// Подготавливает рабочее изображение, уменьшая большие изображения до оптимального размера.
    /// </summary>
    private static Image<L8> PrepareWorkingImage(Image<L8> image, int maxDimension)
    {
        if (maxDimension <= LargeImageThreshold) return image.Clone();
        var targetSize = maxDimension > VeryLargeImageThreshold ? 1200 : 1500;
        return ResizeImage(image, targetSize);
    }

    /// <summary>
    /// Возвращает базовые варианты: оригинал и разные размеры.
    /// </summary>
    private static IEnumerable<Image<L8>> GetBasicVariants(Image<L8> image, int maxDimension)
    {
        yield return image.Clone();

        foreach (var size in ResizeSizes)
        {
            yield return ResizeImage(image, size);
        }

        // Увеличение для мелких повреждений
        if (maxDimension < SmallImageThreshold)
        {
            yield return ResizeImage(image, 1500);
        }
    }

    /// <summary>
    /// Возвращает базовые варианты обработки изображения.
    /// </summary>
    private static IEnumerable<Image<L8>> GetBasicProcessingVariants(Image<L8> image)
    {
        yield return ApplyProcessing(image, blur: 0.5f, contrast: 1.3f);
        yield return ApplyProcessing(image, blur: 1.0f, contrast: 1.5f);
        yield return ApplyProcessing(image, blur: 1.2f, threshold: 0.5f);
        yield return ApplyProcessing(image, blur: 1.5f, threshold: 0.5f);
    }

    /// <summary>
    /// Возвращает агрессивные варианты обработки для сглаживания помарок.
    /// </summary>
    private static IEnumerable<Image<L8>> GetAggressiveProcessingVariants(Image<L8> image)
    {
        yield return ApplyProcessing(image, blur: 2.0f, contrast: 2.0f, threshold: 0.5f);
        yield return ApplyProcessing(image, blur: 2.5f, contrast: 2.5f, threshold: 0.5f);
    }

    /// <summary>
    /// Возвращает варианты с инверсией для темных QR-кодов на светлом фоне.
    /// </summary>
    private static IEnumerable<Image<L8>> GetInvertedVariants(Image<L8> image)
    {
        yield return ApplyProcessing(image, invert: true);
        yield return ApplyProcessing(image, invert: true, blur: 0.8f, contrast: 1.3f);
        yield return ApplyProcessing(image, invert: true, blur: 1.2f, threshold: 0.5f);
        yield return ApplyProcessing(image, invert: true, blur: 2.0f, contrast: 2.0f, threshold: 0.5f);
    }

    /// <summary>
    /// Возвращает варианты с пороговой бинаризацией.
    /// </summary>
    private static IEnumerable<Image<L8>> GetBinaryThresholdVariants(Image<L8> image)
    {
        foreach (var threshold in BinaryThresholds)
        {
            yield return ApplyProcessing(image, threshold: threshold);
        }
    }

    /// <summary>
    /// Возвращает варианты с комбинациями контраста и яркости.
    /// </summary>
    private static IEnumerable<Image<L8>> GetContrastBrightnessVariants(Image<L8> image)
    {
        yield return ApplyProcessing(image, contrast: 1.5f, brightness: 1.1f, threshold: 0.5f);
        yield return ApplyProcessing(image, contrast: 1.2f, brightness: 0.9f, threshold: 0.5f);
        yield return ApplyProcessing(image, contrast: 2.0f, brightness: 1.2f, threshold: 0.5f);
        yield return ApplyProcessing(image, contrast: 2.5f, brightness: 0.8f, threshold: 0.5f);
    }

    /// <summary>
    /// Возвращает комплексные варианты обработки для выцветших кодов.
    /// </summary>
    private static IEnumerable<Image<L8>> GetFadedCodeVariants(Image<L8> image)
    {
        foreach (var blur in BlurValues)
        {
            foreach (var contrast in ContrastValues)
            {
                foreach (var threshold in ThresholdValues)
                {
                    yield return ApplyProcessing(image, blur: blur, contrast: contrast, threshold: threshold);
                }
            }
        }
    }

    /// <summary>
    /// Возвращает агрессивные варианты с инверсией.
    /// </summary>
    private static IEnumerable<Image<L8>> GetAggressiveInvertedVariants(Image<L8> image)
    {
        yield return ApplyProcessing(image, invert: true, blur: 1.5f, contrast: 2.0f, threshold: 0.5f);
        yield return ApplyProcessing(image, invert: true, blur: 2.0f, contrast: 2.5f, threshold: 0.5f);
    }

    /// <summary>
    /// Возвращает варианты с множественными проходами размытия.
    /// </summary>
    private static IEnumerable<Image<L8>> GetDoubleBlurVariants(Image<L8> image)
    {
        yield return image.Clone(ctx =>
        {
            ctx.GaussianBlur(1.0f);
            ctx.GaussianBlur(1.0f); // Двойное размытие
            ctx.Contrast(2.0f);
            ctx.BinaryThreshold(0.5f);
        });
    }

    /// <summary>
    /// Возвращает варианты обработки обрезанных областей.
    /// </summary>
    private static IEnumerable<Image<L8>> GetCroppedRegionVariants(Image<L8> image)
    {
        if (image.Width <= CroppedRegionThreshold && image.Height <= CroppedRegionThreshold)
            yield break;

        foreach (var cropped in GetCroppedRegions(image))
        {
            using (cropped)
            {
                yield return cropped.Clone();
                yield return ResizeImage(cropped, CroppedRegionThreshold);

                // Базовые варианты для обрезанных областей
                yield return ApplyProcessing(cropped, blur: 0.8f, contrast: 1.3f);
                yield return ApplyProcessing(cropped, blur: 1.2f, threshold: 0.5f);
                yield return ApplyProcessing(cropped, invert: true);
                yield return ApplyProcessing(cropped, threshold: 0.5f);

                // Агрессивная обработка для обрезанных областей
                yield return ApplyProcessing(cropped, blur: 2.0f, contrast: 2.0f, threshold: 0.5f);
                yield return ApplyProcessing(cropped, invert: true, blur: 1.5f, contrast: 2.0f, threshold: 0.5f);

                // Тонкие пороги для обрезанных областей
                foreach (var threshold in CroppedBinaryThresholds)
                {
                    yield return ApplyProcessing(cropped, threshold: threshold);
                }
            }
        }
    }

    /// <summary>
    /// Применяет обработку к изображению с указанными параметрами.
    /// </summary>
    private static Image<L8> ApplyProcessing(
        Image<L8> image,
        bool invert = false,
        float? blur = null,
        float? contrast = null,
        float? brightness = null,
        float? threshold = null)
    {
        return image.Clone(ctx =>
        {
            if (invert)
                ctx.Invert();

            if (blur.HasValue)
                ctx.GaussianBlur(blur.Value);

            if (contrast.HasValue)
                ctx.Contrast(contrast.Value);

            if (brightness.HasValue)
                ctx.Brightness(brightness.Value);

            if (threshold.HasValue)
                ctx.BinaryThreshold(threshold.Value);
        });
    }

    /// <summary>
    /// Уменьшает изображение до указанного максимального размера по большей стороне.
    /// </summary>
    private static Image<L8> ResizeImage(Image<L8> image, int maxSize)
    {
        var maxDimension = Math.Max(image.Width, image.Height);
        if (maxDimension <= maxSize)
            return image.Clone();

        var scale = (double)maxSize / maxDimension;
        var newWidth = (int)Math.Round(image.Width * scale);
        var newHeight = (int)Math.Round(image.Height * scale);

        return image.Clone(ctx =>
        {
            ctx.Resize(new ResizeOptions
            {
                Size = new Size(newWidth, newHeight),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Lanczos3,
            });
        });
    }

    /// <summary>
    /// Получает несколько обрезанных областей для поиска QR-кода в разных частях изображения.
    /// </summary>
    private static IEnumerable<Image<L8>> GetCroppedRegions(Image<L8> src)
    {
        foreach (var (widthFactor, heightFactor, xOffset, yOffset) in CropRegionParameters)
        {
            yield return CropRegion(src, widthFactor, heightFactor, xOffset, yOffset);
        }
    }

    private static Image<L8> CropRegion(Image<L8> src, float widthFactor, float heightFactor, float xOffset, float yOffset)
    {
        var width = (int)(src.Width * widthFactor);
        var height = (int)(src.Height * heightFactor);
        var x = (int)(src.Width * xOffset);
        var y = (int)(src.Height * yOffset);

        width = Math.Min(width, src.Width - x);
        height = Math.Min(height, src.Height - y);

        if (width <= 0 || height <= 0)
            return src.Clone();

        return src.Clone(c => c.Crop(new Rectangle(x, y, width, height)));
    }
}
